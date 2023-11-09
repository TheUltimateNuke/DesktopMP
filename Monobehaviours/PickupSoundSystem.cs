using System.Collections;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class PickupSoundSystem : MonoBehaviour
{

    public PickupSoundSystem(IntPtr ptr) : base(ptr) { }

    public AudioClip[] pickupSounds;

    public float soundDelay = 0.5f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public IEnumerator PlayRandomSound()
    {
        yield return new WaitForSeconds(Random.Range(0f, soundDelay));

        AudioClip clipToPlay = pickupSounds[Random.Range(0, pickupSounds.Length)];
        audioSource.clip = clipToPlay;
        audioSource.Play();
    }

}