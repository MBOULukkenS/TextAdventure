using System.Linq;
using UnityEngine;
using Utility;

namespace Managers
{
    /// <summary>
    /// Dit object regelt de geluidseffecten in het spel.
    /// </summary>
    public class SoundSystem : MonoSingleton<SoundSystem>
    {
        public AudioSource AudioSource { get; private set; }

        public override void Start()
        {
            AudioSource = gameObject.AddComponent<AudioSource>();
        }

        public void PlaySound(string clipName, int volume = 100)
        {
            AudioClip clipToPlay = GetAudioClipByName(clipName);
            
            AudioSource.PlayOneShot(clipToPlay, (volume / 100f));
        }

        private static AudioClip GetAudioClipByName(string name)
        {
            return SoundConfig.Instance.AudioClips.First(ac => ac.name == name);
        }
    }
}