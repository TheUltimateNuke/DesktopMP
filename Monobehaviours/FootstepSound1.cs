using UnityEngine;
using System;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
[RequireComponent(typeof(Rigidbody))]
#endif
public class FootstepSound1 : MonoBehaviour
{
    public FootstepSound1(IntPtr ptr) : base(ptr) { }

#if UNITY_EDITOR
    [Tooltip("The minimum velocity required to play a footstep sound.")]
#endif
    public float minVelocity = 1f;

#if UNITY_EDITOR
    [Tooltip("The maximum velocity required to play a footstep sound.")]
#endif
    
    public float maxVelocity = 10f;

#if UNITY_EDITOR
    [Tooltip("The delay between playing footstep sounds.")]
#endif
    public float footstepDelay = 0.5f;

#if UNITY_EDITOR
    [Tooltip("The audio clips for footstep sounds.")]
#endif
    public AudioClip[] footstepSounds;

#if UNITY_EDITOR
    [Tooltip("The audio clips for jump sounds.")]
#endif
    
    public AudioClip[] jumpSounds;

    private Rigidbody rb;
    private AudioSource audioSource;
    private float lastFootstepTime;
    private FPSController controller;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<FPSController>();
        audioSource = GetComponent<AudioSource>();
        lastFootstepTime = Time.time;
    }

    private void Update()
    {
        float horizontalInput = controller.GetHorizontal();
        float verticalInput = controller.GetVertical();
        float inputStrength = Mathf.Abs(horizontalInput + verticalInput);

        if (controller.IsGrounded() && inputStrength > minVelocity && inputStrength < maxVelocity)
        {
            if (Time.time - lastFootstepTime > footstepDelay)
            {
                PlayFootstepSound();
                lastFootstepTime = Time.time;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.IsGrounded())
        {
            PlayJumpSound();
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0)
        {
            int index = Random.Range(0, footstepSounds.Length);
            audioSource.clip = footstepSounds[index];
            audioSource.Play();
        }
    }

    private void PlayJumpSound()
    {
        if (jumpSounds.Length > 0)
        {
            int index = Random.Range(0, jumpSounds.Length);
            audioSource.clip = jumpSounds[index];
            audioSource.Play();
        }
    }
}