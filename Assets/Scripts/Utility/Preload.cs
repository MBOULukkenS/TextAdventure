using Managers;
using UnityEngine;

namespace Utility
{
    public class Preload
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void LoadAfterScene()
        {
            GameManager.CreateInstance();
            SoundSystem.CreateInstance();
        }
    }
}