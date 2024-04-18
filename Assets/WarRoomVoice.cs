using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarRoomVoice : MonoBehaviour
{
    public AudioClip voiceLine;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        if (gameObject.GetComponent<AudioSource>() == null){
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.clip = voiceLine;
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            audioSource.Play();
        }
    }
}
