using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WM2000.Display
{
    public class Display : MonoBehaviour
    {
        [SerializeField] 
        private Terminal.Terminal connectedToTerminal;

        // TODO: calculate these two if possible
        [SerializeField] 
        private int charactersWide = 40;
        
        [SerializeField] 
        private int charactersHigh = 14;

        private TextMeshProUGUI _screenText;

        private void Start()
        {
            _screenText = GetComponentInChildren<TextMeshProUGUI>();
            WarnIfTerminalNotConnected();
        }

        private void WarnIfTerminalNotConnected()
        {
            if (!connectedToTerminal)
            {
                Debug.LogWarning("Display not connected to a terminal");
            }
        }

        // Akin to monitor refresh
        private void Update()
        {
            if (connectedToTerminal)
            {
                _screenText.text = connectedToTerminal.GetDisplayBuffer(charactersWide, charactersHigh);
            }
        }
    }
} 