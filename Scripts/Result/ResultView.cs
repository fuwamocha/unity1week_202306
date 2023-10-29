using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.InGame.Score;
using MochaLib.Audio;
using MochaLib.Settings;
using TMPro;
using TweetWithScreenShot;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Scripts.Result
{
    public class ResultView : MonoBehaviour
    {
        private static readonly int Flip = Shader.PropertyToID("_Flip");
        [SerializeField] private Button _tweetButton;
        [SerializeField] private Transform _rauTransform;
        [SerializeField] private GameObject _waffle;
        [SerializeField] private Transform _waffleTra;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _backToTitleButton;
        [SerializeField] private Material _flipMaterial;
        [SerializeField] private GameObject _resultText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _comboText;
        [SerializeField] private TextMeshProUGUI _waffleCountText;
        [SerializeField] private TextMeshProUGUI _typeText;
        // private DoyoAction _doyoAction;
        private Vector3 _originWafflePosition;
        private GameObject _rauLive2D;
        private float _value;
        private GameObject _wafflePrefab;
        private void Awake()
        {
            _tweetButton.onClick.AddListener(() =>
            {
                StartCoroutine(TweetManager.TweetWithScreenShot($"今日のらうちゃんは{ScoreManager.Score}円稼ぎました。やったネ！"));
            });
            _rauLive2D = GameObject.FindWithTag("RauLive2D");
            // _doyoAction = _rauLive2D.GetComponent<DoyoAction>();
            ResetStatus();
        }
        public void UpdateText()
        {
            _scoreText.text = $"合計: {ScoreManager.Score.Value.ToString()}円";
            ScoreManager.SendScore(ScoreManager.Score.Value);
            _comboText.text = $"連続提供数: {ScoreManager.Combo.Value.ToString()}";
            _waffleCountText.text = $"提供数: {ScoreManager.SupplyWaffleCount.ToString()}";
            _typeText.text = ScoreManager.Score.Value switch
            {
                <= 0      => "サボリ級",
                <= 30000  => "研修生級",
                <= 70000  => "職人級",
                <= 100000 => "マスター級",
                <= 150000 => "レジェンド級",
                <= 300000 => "アルティメット級",
                _         => _typeText.text
            };
        }

        public async UniTask Show(AudioPlayer audioPlayer)
        {
            gameObject.SetActive(true);
            audioPlayer.PlaySe(CommonConstants.Audio.Se.Flip);
            await AnimateFloat(-1f, 1f);
            _resultText.SetActive(true);
            audioPlayer.PlaySe(CommonConstants.Audio.Se.Don);
            await UniTask.Delay(1000);
            _scoreText.enabled = true;
            _comboText.enabled = true;
            _waffleCountText.enabled = true;
            audioPlayer.PlaySe(CommonConstants.Audio.Se.Don);
            await UniTask.Delay(1000);
            _typeText.enabled = true;
            _retryButton.gameObject.SetActive(true);
            _backToTitleButton.gameObject.SetActive(true);
            audioPlayer.PlaySe(CommonConstants.Audio.Se.Dodon);
            _tweetButton.gameObject.SetActive(true);
        }

        public async UniTask MoveCharacter()
        {
            await UniTask.Delay(1000);
            _rauLive2D.transform.parent = _rauTransform;
            _rauLive2D.transform.localScale = Vector3.one * 780f;
            await _rauLive2D.transform.DOMoveY(-2f, 0.5f);
            await _rauLive2D.transform.DOMoveX(3f, 0.5f);
            await UniTask.Delay(2000);
            // _doyoAction.Animate(CommonConstants.Character.Animation.JyanAge);
        }
        private async UniTask ResultMoveCharacter()
        {
            await _rauLive2D.transform.DOMoveX(15f, 0.8f);
            await _wafflePrefab.transform.DOMove(_wafflePrefab.transform.position + Vector3.right * 15f, 0.8f);
            await UniTask.Delay(800);
            Destroy(_wafflePrefab);
        }

        private async UniTask AnimateFloat(float start, float end)
        {
            const float duration = 0.5f;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                _value = Mathf.Lerp(start, end, t / duration);
                _flipMaterial.SetFloat(Flip, _value);
                await UniTask.Yield();
            }
            _flipMaterial.SetFloat(Flip, end);
            _value = end;
        }

        public void ResetStatus()
        {
            gameObject.SetActive(false);
            _retryButton.gameObject.SetActive(false);
            _backToTitleButton.gameObject.SetActive(false);
            _resultText.SetActive(false);
            _scoreText.enabled = false;
            _comboText.enabled = false;
            _waffleCountText.enabled = false;
            _typeText.enabled = false;

            _waffle.transform.localScale = Vector3.one;
            _wafflePrefab = Instantiate(_waffle, _waffleTra);

            _flipMaterial.SetFloat(Flip, -1f);
        }
        public async UniTask Hide()
        {
            // _doyoAction.Animate(CommonConstants.Character.Animation.JyanOroshi);
            // await _doyoAction.WaitUntilAnimationEnd();
            // _doyoAction.Animate(CommonConstants.Character.Animation.NoTentacles);
            _tweetButton.gameObject.SetActive(false);
            _retryButton.gameObject.SetActive(false);
            _backToTitleButton.gameObject.SetActive(false);
            _resultText.SetActive(false);
            _scoreText.enabled = false;
            _comboText.enabled = false;
            _waffleCountText.enabled = false;
            _typeText.enabled = false;
            await AnimateFloat(1f, -1f);
            await ResultMoveCharacter();
            gameObject.SetActive(false);
        }

        public IObservable<Unit> OnClickRetryAsObservable() => _retryButton.OnClickAsObservable();

        public IObservable<Unit> OnClickBackToTitleAsObservable() => _backToTitleButton.OnClickAsObservable();
    }
}
