using UnityEngine;
using Utility;

/// <summary>
/// Geluids Configuratie bestand.
/// </summary>
[CreateAssetMenu(fileName = nameof(SoundConfig), menuName = "Configs/Sound Configuration File", order = 0)]
public class SoundConfig : ScriptableObjectConfigSingleton<SoundConfig>
{
    public AudioClip[] AudioClips;
}