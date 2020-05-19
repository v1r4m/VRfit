﻿using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Including BPM counter, etc
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public float delayedPlay = 3;
    private double sampleRate;

    public float readOnlyBeatTest;

    public float CurrentBeat { get { return (float)BPM / 60 * (@as.time - offset/1000f); } }
    
    public float currntBPM = 326.975476839237f;
    public int offset = 188;
    public float BPM
    {
        set { currntBPM = value; }
        get { return currntBPM; }
    }


    AudioSource @as;


    // Use this for initialization
    void Start()
    {
        var allPlayers = FindObjectsOfType<MusicPlayer>();
        @as = GetComponent<AudioSource>();
        sampleRate = AudioSettings.outputSampleRate; //hack: is this right or do I have to divide by channels?

        
    }

    public AudioClip tick, tock;
    int metronomePlayed = 0;
    void Update()
    {
        readOnlyBeatTest = CurrentBeat;

        
        if (metronomePlayed < CurrentBeat)
        {
            if (metronomePlayed % 4 == 0)
                if(tick != null)
                    @as.PlayOneShot(tick, 10);
            else
                if (tock != null)
                    @as.PlayOneShot(tock, 6);
            metronomePlayed++;
        }
        

    }
}
