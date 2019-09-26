using UnityEngine;

namespace DefaultNamespace
{
    public static class Globals
    {
        public static char[] Separator { get; set; } = {' '};

        public const KeyCode SkipKey = KeyCode.Escape;
        
        public const KeyCode HistoryPreviousKey = KeyCode.UpArrow;
        public const KeyCode HistoryNextKey = KeyCode.DownArrow;
    }
}