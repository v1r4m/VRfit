using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using RhythmTool;
using System.Diagnostics;
using System.Text;

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
    public float lookaheadtime;

    private float prevTime;
    private List<Beat> beats;
    private List<Onset> onsetFeatures;

    private List<Note> randomNote = new List<Note>();

    public static float hr;
    public float hrthres = 120;

    public Transform zero, two;
    public TextAsset inputCsv;

    bool AutoConstruct = true;

    private void OnBeat(Beat beat)
    {
        UnityEngine.Debug.Log("A beat occurred at " + beat.timestamp);
    }

    void OnDestroy()
    {
        eventProvider.Unregister<Beat>(OnBeat);
    }

    public float targetSPN = 1;
    public float hrWeight = 0.001f;
    float? lastDelta = null;
    float? avgDelta = null;
    int scannedUntil = 0;
    TMPro.TMP_Text txt;
    void Awake()
    {
        txt = GameObject.Find("HRText").GetComponent<TMPro.TMP_Text>();
        beats = new List<Beat>();
        onsetFeatures = new List<Onset>();
        musicPlayer = GetComponent<MusicPlayer>();
        audioSource = GetComponent<AudioSource>();
        rhythmPlayer = GetComponent<RhythmPlayer>();

        rhythmData.GetIntersectingFeatures(onsetFeatures,0,180);
        rhythmData.GetIntersectingFeatures(beats, 0, 180);

        UnityEngine.Debug.Log(onsetFeatures);

        
        foreach (var onset in onsetFeatures) //여기서생성
        {
            SpawnRandomNote(onset.timestamp);
        }


        rhythmPlayer.Play();
    }
    StringBuilder sb = new StringBuilder("hr, targetSPN(difficulty), avgDelta (real difficulty), hrthres\n");

    string threeFloat(float f)
    { return string.Format("{0:f3}", f); }


    void FixedUpdate()
    {
        txt.text = "heart rate: " + hr  + "\nTarget HR = " + hrthres;
        txt.text += "\nTarget Difficulty : " + threeFloat(targetSPN) + "\n<sub>Real Difficulty : " + threeFloat(avgDelta??0);
        txt.text += "\nAdjusted Difficulty : " + threeFloat(targetSPN + (targetSPN - avgDelta ?? 0));
        float time = musicPlayer.CurrentBeat;
        targetSPN -= (hrthres - hr) * hrWeight * Time.deltaTime;
        if (targetSPN < 0)
            targetSPN = 0;
        if (targetSPN > 10)
            targetSPN = 10;
        while (randomNote[scannedUntil] == null)
            scannedUntil++;

        while (randomNote[scannedUntil].beat < time + lookaheadtime && scannedUntil + 2 < randomNote.Count)
        {
            float noteDelta = randomNote[scannedUntil + 1].beat - randomNote[scannedUntil].beat;
            float noteDeltaifDropped = randomNote[scannedUntil + 2].beat - randomNote[scannedUntil].beat;

            float adjusted = targetSPN + (targetSPN - avgDelta ?? targetSPN);

            float DeltaTarget = Mathf.Abs(adjusted - noteDelta);
            float ifDropDeltaTarget = Mathf.Abs(adjusted - noteDeltaifDropped);

            if (DeltaTarget > ifDropDeltaTarget)
            {
                Destroy(randomNote[scannedUntil + 1].gameObject);
                randomNote.RemoveAt(scannedUntil + 1);
                DeltaTarget = ifDropDeltaTarget;
                UnityEngine.Debug.Log("Dropped one!");
                continue;
            }

            lastDelta = DeltaTarget;
            UnityEngine.Debug.Log("ad = " + avgDelta);
            avgDelta = (avgDelta ?? DeltaTarget) * 0.5f + noteDelta * 0.5f; // LPS
            UnityEngine.Debug.Log("ad = " + avgDelta + " and delt = " + noteDelta);
            sb.Append(string.Format("{0},{1},{2},{3}\n", hr, targetSPN/1, avgDelta ?? 0, hrthres));
            scannedUntil++;
        }


    }
    void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("writing logs");
        File.WriteAllText("log.csv",sb.ToString());
    }

    Note SpawnRandomNote(float beattime)
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

        n.Init(beattime, rnd, musicPlayer, 1, handSide, hookSide);
        n.reqStrength = 0;
        randomNote.Add(n);

        return n;
    }

    FootNote SpawnRandomFootNote(float beattime)
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

        n.Init(beattime, rnd, musicPlayer, 1, handSide, hookSide);
        return n;
    }

}
