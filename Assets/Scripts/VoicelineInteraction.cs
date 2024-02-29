using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class VoicelineInteraction : MonoBehaviour

{
    public Transform player;
    public AudioClip voiceLine;
    public float distanceThreshold = 2f;
    private AudioSource audioSource;
    private bool playLine = true;
    private float distance;
    // Start is called before the first frame update
    void Awake()
    {
        if (gameObject.GetComponent<AudioSource>() == null){
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.clip = voiceLine;
        playLine = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(gameObject.transform.position, player.position);
        Debug.Log("Distance between player and " + gameObject.name + " " + distance);
        if(!audioSource.isPlaying && Input.GetKey(KeyCode.E) && CheckProximity()) {
            audioSource.Play();
        }
    }

    bool CheckProximity() {
        return distance <= distanceThreshold;
    }
}
