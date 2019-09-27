using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = nameof(SoundConfig), menuName = "Configs/Sound Configuration File", order = 0)]
public class SoundConfig : ScriptableObjectConfigSingleton<SoundConfig>
{
    public AudioClip[] AudioClips;
}