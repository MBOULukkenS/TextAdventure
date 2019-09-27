using Managers;
using UnityEngine;

namespace Utility
{
    public class Preload
    {
        /// <summary>
        /// Functie die wordt uitgevoerd nadat de eerste scene is geladen. Handig voor initialisatie.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void LoadAfterScene()
        {
            GameManager.CreateInstance();
            SoundSystem.CreateInstance();
            
            InitGame();
        }

        /// <summary>
        /// Initialiseert game instellingen.
        /// </summary>
        static void InitGame()
        {
            QualitySettings.vSyncCount = 0; // No V-Sync so Update() not held back by render
            Application.targetFrameRate = 1000; // To minimise delay playing key sounds
        }
    }
}