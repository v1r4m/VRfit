using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using RhythmTool;
using System.Diagnostics;

public class NoteSpawner : MonoBehaviour
{
    public GameObject note, duckNote, footNote;
    MusicPlayer musicPlayer;
    public RhythmEventProvider eventProvider;
//    public string path;
    public AudioImporter importer;
    public RhythmAnalyzer analyzer;
    public RhythmData rhythmData;
    public RhythmPlayer rhythmPlayer;
    private AudioSource audioSource;

    private float prevTime;
    private List<Beat> beats;
    private List<Chroma> chromaFeatures;

    private List<Note> randomNote;

    public static float hr;
    public float hrthres = 50;

    public Transform zero, two;
    public TextAsset inputCsv;

    bool AutoConstruct = false;
    // Start is called before the first frame update
    void Start()
    {


        AutoConstruct = true;
    }
    public float lastNoteSpawned = 0;

    private void OnBeat(Beat beat)
    {
        UnityEngine.Debug.Log("A beat occurred at " + beat.timestamp);
    }

    void OnDestroy()
    {
        eventProvider.Unregister<Beat>(OnBeat);
    }

    // Update is called once per frame
    void Update()
    {
        float time = musicPlayer.CurrentBeat;

    }
    void SpawnRandomFootNote()
    {
        var n = Instantiate(footNote, Vector3.zero, Quaternion.identity).GetComponent<FootNote>();
        Vector2 rnd = new Vector2(Random.Range(-.7f, .7f), 0);
        HandSide handSide = HandSide.any;
        float? hookSide = null;
        if (Random.Range(0, 20) < 1)
            handSide = HandSide.left;
        else if (Random.Range(0, 19) < 1)
            handSide = HandSide.right;
        else
        {
            switch (Random.Range(0, 20))
            {
                case 0: hookSide = 0; break;
                case 1: hookSide = 90; break;
                case 2: hookSide = 270; break;
                case 3: hookSide = 180; break;
                default: hookSide = null; break;
            }
        }

        n.Init(lastNoteSpawned, rnd, musicPlayer, 1, handSide, hookSide);
    }

    void Awake()
    {
        analyzer.Initialized += OnInitialized;


        beats = new List<Beat>();
        chromaFeatures = new List<Chroma>();
        eventProvider.Register<Onset>(OnOnset);
        musicPlayer = GetComponent<MusicPlayer>();
        eventProvider.Register<Beat>(OnBeat);
        audioSource = GetComponent<AudioSource>();
        rhythmPlayer = GetComponent<RhythmPlayer>();

        rhythmData.GetIntersectingFeatures<Chroma>(chromaFeatures,0,180);

        UnityEngine.Debug.Log(chromaFeatures);
        //        rhythmPlayer.Reset += OnReset;
        foreach(Chroma chroma in chromaFeatures) //여기서생성
        {
            UnityEngine.Debug.Log("chroma features occured at " + chroma.timestamp);

            //chroma : length, note timestamp
            SpawnRandomNote(chroma.timestamp);
        }

    }

    private void OnInitialized(RhythmData rhythmData)
    {
        //Start playing the song.
        rhythmPlayer.Play();
    }
    private void OnOnset(Onset onset) //안쓰는거
    {
        UnityEngine.Debug.Log("on onset");
        //Clear any previous Chroma features.
        chromaFeatures.Clear();

        //Find Chroma features that intersect the Onset's timestamp.
        rhythmPlayer.rhythmData.GetIntersectingFeatures(chromaFeatures, onset.timestamp, onset.timestamp);

        //Instantiate a line to represent the Onset and Chroma feature.
        foreach (Chroma chroma in chromaFeatures)
        {
            if (onset.strength > 1)
            {
                SpawnRandomNote(onset.timestamp);
                UnityEngine.Debug.Log("called" + onset.timestamp);
//CreateLine(onset.timestamp, -2 + (float)chroma.note * .1f, 0.3f, Color.blue, onset.strength / 10);
            }
            UnityEngine.Debug.Log(onset.strength);
        }

        if (chromaFeatures.Count > 0)
//            lastNote = chromaFeatures[chromaFeatures.Count - 1].note;

        //If no Chroma Feature was found, use the last known Chroma feature's note.
        if (chromaFeatures.Count == 0)
            SpawnRandomNote(onset.timestamp);
//        CreateLine(onset.timestamp, -2 + (float)lastNote * .1f, 0.3f, Color.blue, onset.strength / 10);
    }
    private void OnLoaded(AudioClip clip)
    {
        analyzer.Analyze(clip);
    }

    void SpawnRandomNote(float beattime)
    {
        var n = Instantiate(note, Vector3.zero, Quaternion.identity).GetComponent<Note>();

        Vector2 rnd = Random.insideUnitCircle * .6f + new Vector2(0, 1f);
        HandSide handSide = HandSide.any;
        float? hookSide = null;
        if (Random.Range(0, 20) < 1)
            handSide = HandSide.left;
        else if (Random.Range(0, 19) < 1)
            handSide = HandSide.right;
        else
        {
            switch (Random.Range(0, 20))
            {
                case 0: hookSide = 0; break;
                case 1: hookSide = 90; break;
                case 2: hookSide = 270; break;
                case 3: hookSide = 180; break;
                default: hookSide = null; break;
            }
        }

        n.Init(beattime*2, rnd, musicPlayer, 1, handSide, hookSide);
        n.reqStrength = 0;
    }
}
