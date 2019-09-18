using UnityEngine;

namespace WM2000.Keyboard
{
    public class Keyboard : MonoBehaviour
    {
        [SerializeField] 
        private AudioClip[] keyStrokeSounds;
        
        [SerializeField] 
        private Terminal.Terminal connectedToTerminal;

        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            QualitySettings.vSyncCount = 0; // No V-Sync so Update() not held back by render
            Application.targetFrameRate = 1000; // To minimise delay playing key sounds
            WarnIfTerminalNotConnected();
        }

        private void WarnIfTerminalNotConnected()
        {
            if (!connectedToTerminal)
            {
                Debug.LogWarning("Keyboard not connected to a terminal");
            }
        }

        private void Update()
        {
            bool isValidKey = Input.inputString.Length > 0;
            if (isValidKey)
            {
                PlayRandomSound();
            }
            if (connectedToTerminal)
            {
                connectedToTerminal.ReceiveFrameInput(Input.inputString);
            }
        }

        private void PlayRandomSound()
        {
            int randomIndex = Random.Range(0, keyStrokeSounds.Length);
            _audioSource.clip = keyStrokeSounds[randomIndex];
            _audioSource.Play();
        }
    }
}
