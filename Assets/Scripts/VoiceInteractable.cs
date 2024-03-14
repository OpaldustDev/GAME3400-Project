using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceInteractable : MonoBehaviour, Interactables
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
    public void Interact()
    {
        audioSource.Play();
    }
}
