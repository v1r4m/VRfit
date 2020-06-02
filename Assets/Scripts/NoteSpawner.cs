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
            SpawnRandomFootNote(onset.timestamp).importance = onset.strength;
            SpawnRandomNote(onset.timestamp).importance = onset.strength;
        }


        rhythmPlayer.Play();
    }



    string BuildPointThreeFloat(float f)
    { return string.Format("{0:f3}", f); }

    int lookupDist = 10;

    StringBuilder sb = new StringBuilder("hr, targetSPN(difficulty), avgDelta (real difficulty), hrthres\n");
    float hrDiff = 0;
    float lastHr;
    void FixedUpdate()
    {
        txt.text = "heart rate: " + hr  + "\nTarget HR = " + hrthres;
        txt.text += "\nTarget Difficulty : " + BuildPointThreeFloat(targetSPN) + "\n<sub>Real Difficulty : " + BuildPointThreeFloat(avgDelta??0);
        txt.text += "\nAdjusted Difficulty : " + BuildPointThreeFloat(targetSPN + (targetSPN - avgDelta ?? 0));
        txt.text += "\nHD : " + BuildPointThreeFloat(targetSPN + (targetSPN - avgDelta ?? 0));
        float time = musicPlayer.CurrentBeat;
        hrDiff = (hr - lastHr) * 0.1f + hrDiff * 0.9f;
        lastHr = hr;
        float hrAdjWeight;
        if (
            hrDiff < -0.1f && hr < hrDiff // hr이 떨어지고 있고, hr이 hrDiff보다 낮음
            ||
            hrDiff > 0.1f && hr > hrDiff // hr이 떨어지고 있고, hr이 hrDiff보다 낮음
            ) hrAdjWeight = 0.01f;// 난이도 바꾸는속도 줄임
        else
            hrAdjWeight = 1;
         targetSPN -= (hrthres - hr) * hrWeight * hrAdjWeight * Time.fixedDeltaTime;

        if (targetSPN < 0.1)
            targetSPN = 0.1f;
        if (targetSPN > 10)
            targetSPN = 10;
        
        while (scannedUntil + lookupDist < randomNote.Count && randomNote[scannedUntil] == null)
            scannedUntil++;

        float adjustedDiff = targetSPN + (targetSPN - avgDelta ?? targetSPN);

        float noteDeltaTargetDiff = 0;

        int seqDrop = 0;
        bool loop = false;
        while (randomNote[scannedUntil].beat < time + lookaheadtime && scannedUntil + lookupDist < randomNote.Count)
        {
            loop = true;
            float noteDeltaSum = 0;
            noteDeltaSum = randomNote[scannedUntil + lookupDist].beat - randomNote[scannedUntil].beat;

            float noteDeltaAvg = noteDeltaSum / 10;
            float noteDeltaAvgifDropped = noteDeltaSum / 9; 


            noteDeltaTargetDiff = Mathf.Abs(adjustedDiff - noteDeltaAvg);
            float ifDropDiff = Mathf.Abs(adjustedDiff - noteDeltaAvgifDropped);

            if (noteDeltaTargetDiff > ifDropDiff && seqDrop < 3) 
            {
                seqDrop++;
                UnityEngine.Debug.Log(string.Format("{0},{1},{2},{3}\n", scannedUntil, targetSPN / 1, avgDelta ?? 0, hrthres));
                int selectedIndex = scannedUntil + 1;
                for (int i = scannedUntil + 2; i < scannedUntil + lookupDist; i++)
                {
                    if (randomNote[i].importance < randomNote[selectedIndex].importance)
                        selectedIndex = i;
                }
                Destroy(randomNote[selectedIndex].gameObject);
                randomNote.RemoveAt(selectedIndex);
                noteDeltaTargetDiff = ifDropDiff;
                UnityEngine.Debug.Log("Dropped one!");
                continue;
            }
            seqDrop = 0;
            lastDelta = noteDeltaTargetDiff;
            scannedUntil++;
        }
        if(loop)
            avgDelta = (avgDelta ?? targetSPN) * 0.5f + noteDeltaTargetDiff * 0.5f; // LPS
        sb.Append(string.Format("{0},{1},{2},{3}\n", hr, targetSPN / 1, avgDelta ?? 0, hrthres));


    }
    void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("writing logs");
        File.WriteAllText("log.csv",sb.ToString());
    }

    float heightInMeters = 1.7f;
    Note SpawnRandomNote(float beattime)
    {
        var n = Instantiate(note, Vector3.zero, Quaternion.identity).GetComponent<Note>();

        float shoulderHeight = heightInMeters * 7 / 8;
        float armLength = heightInMeters / 4;
        Vector2 rnd = Random.insideUnitCircle * armLength + new Vector2(0, shoulderHeight);

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
        randomNote.Add(n);
        return n;
    }

}
