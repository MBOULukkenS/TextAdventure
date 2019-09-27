using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WM2000.Terminal;

public class Display : MonoBehaviour
{
    [SerializeField] Terminal connectedToTerminal;
    
    public int DisplayWidth => 
        Mathf.FloorToInt((((RectTransform)_screenText.transform).rect.width * _screenText.pixelsPerUnit) 
                         / (_screenText.fontSize * _screenText.pixelsPerUnit));
    public int DisplayHeight => 
        Mathf.FloorToInt((((RectTransform)_screenText.transform).rect.height * _screenText.pixelsPerUnit) 
                         / (_screenText.fontSize * _screenText.pixelsPerUnit));

    private TextMeshProUGUI _screenText;

    private void Start()
    {
        _screenText = GetComponentInChildren<TextMeshProUGUI>();
        connectedToTerminal.Initialize(DisplayWidth, DisplayHeight);

        if (!connectedToTerminal)
            Debug.LogWarning("Display not connected to a terminal");
    }

    // Akin to monitor refresh
    private void Update()
    {
        if (connectedToTerminal)
            _screenText.text = connectedToTerminal.GetDisplayBuffer();
    }
} 