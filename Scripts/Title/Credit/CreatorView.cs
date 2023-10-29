using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Scripts.Title.Credit
{
    public class CreatorView : MonoBehaviour
    {
        [SerializeField] private Button _fuwamochaButton;
        [SerializeField] private Button _rauButton;
        public IObservable<Unit> OnClickFuwamochaAsObservable() => _fuwamochaButton.OnClickAsObservable();
        public IObservable<Unit> OnClickRauAsObservable() => _rauButton.OnClickAsObservable();
    }
}
