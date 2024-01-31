using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepManager : MonoBehaviour
{
    private AudioSource footstepAudioSource;

    public float minRate = 0.5f; // Minimum time between footstep sounds (in seconds)
    public float maxRate = 0.1f; // Maximum time between footstep sounds (in seconds)

    public float minPitch = 0.9f;  // Minimum pitch for footstep sound
    public float maxPitch = 1.1f;  // Maximum pitch for footstep sound

    private float nextFootstepTime;

    [SerializeField] private Rigidbody playerRb;

    void Awake()
    {
        footstepAudioSource = GetComponent<AudioSource>();
        nextFootstepTime = Time.time;
    }

    // Call this method to play a footstep sound
    public void PlayFootstepSound()
    {
        // Detect the character's velocity (magnitude of the Rigidbody's velocity)
        float characterVelocity = playerRb.velocity.magnitude;

        // Calculate the time between footstep sounds based on velocity
        float timeBetweenFootsteps = Mathf.Lerp(minRate, maxRate, characterVelocity / 10);

        // Check if it's time to play a footstep sound
        if (Time.time >= nextFootstepTime)
        {
            float randomPitch = Random.Range(minPitch, maxPitch);
            footstepAudioSource.pitch = randomPitch;
            footstepAudioSource.Play();

            // Set the time for the next footstep sound
            nextFootstepTime = Time.time + timeBetweenFootsteps;
        }

        
    }
}
