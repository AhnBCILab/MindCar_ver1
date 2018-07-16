using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour {

    //public AudioSource audio;
    //public AudioClip jumpSound;

    //// Use this for initialization
    //void Start () {
    //    this.audio = this.gameObject.AddComponent<AudioSource>();

    //    this.audio.clip = this.jumpSound;
    //    this.audio.loop = false;
    //    this.audio.volume = 1.0f;
    //    this.audio.Play();
    //}
	
    //// Update is called once per frame
    //void Update () {
    //    GetComponent<AudioSource>().Play();
    //}

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSource.Play();
    }
}
