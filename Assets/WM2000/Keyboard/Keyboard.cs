using UnityEngine;
using WM2000.Terminal;

public class Keyboard : MonoBehaviour
{
    [SerializeField] AudioClip[] keyStrokeSounds;
    [SerializeField] Terminal connectedTerminal;

    public int SoundMinimalInterval = 50;
    
    AudioSource _audioSource;
    private float _lastTimePressed = 0;

    private bool ShouldPlayKeyboardSound
    {
        get
        {
            if (Time.time < _lastTimePressed + (SoundMinimalInterval / 1000f)) 
                return false;
            
            _lastTimePressed = Time.time;
            return true;
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (!connectedTerminal) 
            Debug.LogWarning("Keyboard not connected to a terminal");
    }

    private void Update()
    {
        if (!connectedTerminal)
            return;
        
        bool anyKeyPressed = Input.inputString.Length > 0;
        
        if (Input.GetKeyDown(Globals.HistoryPreviousKey))
            connectedTerminal.ReceiveSpecialKeyInput(Globals.HistoryPreviousKey);
        else if (Input.GetKeyDown(Globals.HistoryNextKey))
            connectedTerminal.ReceiveSpecialKeyInput(Globals.HistoryNextKey);

        if (!anyKeyPressed) 
            return;
        connectedTerminal.ReceiveFrameInput(Input.inputString);

        if (Terminal.InputBufferCharCount > 0 && ShouldPlayKeyboardSound)
            PlayRandomSound();
    }

    private void PlayRandomSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, keyStrokeSounds.Length);
        _audioSource.clip = keyStrokeSounds[randomIndex];
        _audioSource.Play();
    }
}
