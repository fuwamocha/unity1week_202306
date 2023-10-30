using TMPro;
using UnityEngine;
namespace MochaLib.Settings
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class EngineVersionText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _engineVersionText;
        private void Reset()
        {
            _engineVersionText = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _engineVersionText.text = $"Ver. {Application.version}";
        }
    }
}
