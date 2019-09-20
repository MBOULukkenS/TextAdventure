using System;
using UnityEngine;
using UnityEngine.Assertions;
using WM2000.Terminal;

public class Keyboard : MonoBehaviour
{
    [SerializeField] AudioClip[] keyStrokeSounds;
    [SerializeField] Terminal connectedToTerminal;

    AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        QualitySettings.vSyncCount = 0; // No V-Sync so Update() not held back by render
        Application.targetFrameRate = 1000; // To minimise delay playing key sounds
        WarnIfTerminalNotConneced();
    }

    private void WarnIfTerminalNotConneced()
    {
        if (!connectedToTerminal) 
            Debug.LogWarning("Keyboard not connected to a terminal");
    }

    private void Update()
    {
        bool anyKeyPressed = Input.inputString.Length > 0;

        if (!anyKeyPressed)
            return;

        if (connectedToTerminal) 
            connectedToTerminal.ReceiveFrameInput(Input.inputString);
        
        if (Terminal.InputBufferCharCount > 0) 
            PlayRandomSound();
    }

    private void PlayRandomSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, keyStrokeSounds.Length);
        _audioSource.clip = keyStrokeSounds[randomIndex];
        _audioSource.Play();
    }
}
