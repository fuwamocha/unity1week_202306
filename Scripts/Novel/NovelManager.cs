using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Character;
using TMPro;
using UniRx;
using UnityEngine;
namespace MochaLib.Novel
{
    public class NovelManager : MonoBehaviour
    {
        private const char PageSeparate = '&';
        private const string SighPattern = "@sigh";
        private const string SmilePattern = "@smile";
        public static readonly IReactiveProperty<bool> IsEnd = new ReactiveProperty<bool>(false);
        [SerializeField] private TextMeshProUGUI _mainText;
        [SerializeField] private TextAsset _textAsset;
        private INovelAction _actor;
        private bool _isTexting;
        private Queue<string> _pageQueue;
        private string _text;

        public void Init(INovelAction actor)
        {
            _actor = actor;
            _text = _textAsset.text.Replace("\n", "&").Replace("\r", "");
            _pageQueue = SeparateString(_text, PageSeparate);

            ShowNextPage();

            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ =>
                {
                    CheckTexting();
                })
                .AddTo(this);
        }

        private static Queue<string> SeparateString(string str, char sep)
        {
            var queue = new Queue<string>();
            foreach (string l in str.Split(sep)) queue.Enqueue(l);
            return queue;
        }

        private bool ShowNextPage()
        {
            if (_pageQueue.Count <= 0) return false;
            ReadLine(_pageQueue.Dequeue());
            return true;
        }

        private void CheckTexting()
        {
            if (_isTexting)
            {
                _mainText.DOComplete();
            }
            else
            {
                if (ShowNextPage() || IsEnd.Value) return;
                _mainText.text = "";
                IsEnd.Value = true;
            }
        }

        private void ReadLine(string text)
        {
            _mainText.text = "";
            _isTexting = true;
            if (text.EndsWith(SmilePattern))
            {
                _actor?.Smile();
                text = text.Replace(SmilePattern, "");
            }
            else if (text.EndsWith(SighPattern))
            {
                _actor?.Sigh();
                text = text.Replace(SighPattern, "");
            }
            _mainText.DOText(text, 1f, scrambleMode: ScrambleMode.None)
                .OnComplete(() => _isTexting = false)
                .SetLink(gameObject);
        }
    }
}
