using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WM2000.Terminal;

public class Display : MonoBehaviour
{
    [SerializeField] Terminal connectedToTerminal;

    // TODO calculate these two if possible
    public int DisplayWidth => 
        Mathf.FloorToInt((((RectTransform)_screenText.transform).rect.width * _screenText.pixelsPerUnit) / (_screenText.fontSize / 4));
    public int DisplayHeight => 
        Mathf.FloorToInt((((RectTransform)_screenText.transform).rect.height * _screenText.pixelsPerUnit) / (_screenText.fontSize / 2));

    private TextMeshProUGUI _screenText;

    private void Start()
    {
        _screenText = GetComponentInChildren<TextMeshProUGUI>();
        WarnIfTerminalNotConneced();
    }

    private void WarnIfTerminalNotConneced()
    {
        if (!connectedToTerminal) 
            Debug.LogWarning("Display not connected to a terminal");
    }

    // Akin to monitor refresh
    private void Update()
    {
        if (connectedToTerminal)
            _screenText.text = connectedToTerminal.GetDisplayBuffer(DisplayWidth, DisplayHeight);
    }
} 