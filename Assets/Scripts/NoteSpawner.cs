using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public class NoteSpawner : MonoBehaviour
{
    public GameObject note, duckNote, footNote;
    MusicPlayer musicPlayer;

    public Transform zero, two;
    public TextAsset inputCsv;

    bool AutoConstruct = false;
    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<MusicPlayer>();
        
       AutoConstruct = true;
    }
    public float lastNoteSpawned = 0;

    // Update is called once per frame
    void Update()
    {   
        if(AutoConstruct)
            while (lastNoteSpawned-30 < musicPlayer.CurrentBeat)
            {
                if (Random.Range(0, 100) > 50) SpawnRandomFootNote();
                if (Random.Range(0, 100) > 70) SpawnRandomNote();
                lastNoteSpawned++;
            }
        

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

    void SpawnRandomNote()
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

        n.Init(lastNoteSpawned, rnd, musicPlayer, 1, handSide, hookSide);
        n.reqStrength = 0;
    }
}
