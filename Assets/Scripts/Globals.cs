using UnityEngine;

/// <summary>
/// Hier staan alle globale en constante variabelen.
/// </summary>
public static class Globals
{
    public static char[] Separator { get; set; } = {' '};

    public const KeyCode SkipKey = KeyCode.Escape;
        
    public const KeyCode HistoryPreviousKey = KeyCode.UpArrow;
    public const KeyCode HistoryNextKey = KeyCode.DownArrow;
}