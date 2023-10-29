using TMPro;
using UniRx;
using UnityEngine;
namespace Game.Scripts.InGame.Score
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private void Awake()
        {
            ScoreManager.IsReset.Subscribe(isReset =>
            {
                if (isReset)
                {
                    _scoreText.text = "売上：0円";
                }
            });
        }
        private void Reset()
        {
            _scoreText = GetComponent<TextMeshProUGUI>();
        }

        public void SetScore(int score)
        {
            _scoreText.text = $"売上：{score.ToString()}円";
            ScoreManager.SendScore(score);
        }

        public void DeleteScoreText()
        {
            _scoreText.text = "";
        }
    }
}
