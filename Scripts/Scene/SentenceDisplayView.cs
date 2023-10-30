using TMPro;
using UnityEngine;
namespace MochaLib.Scene
{
    public class SentenceDisplayView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _sentence;

        [SerializeField]
        private TextMeshProUGUI _kanaText;

        [SerializeField]
        private Color _enteredColor;

        public void SetSententce(string sentence)
        {
            _sentence.SetText(sentence);
        }

        public void SetKanaText(string enteredKanaText, string notEnteredKanaText)
        {
            string enteredColor = ColorUtility.ToHtmlStringRGB(_enteredColor);
            _kanaText.SetText($"<color=#{enteredColor}>{enteredKanaText}</color>{notEnteredKanaText}");
        }
    }
}
