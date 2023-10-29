using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Scripts.Result
{
    [RequireComponent(typeof(Button))]
    public class BackToTitleView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        public IObservable<Unit> OnClickStartAsObservable() => _button.OnClickAsObservable().ThrottleFirst(TimeSpan.FromMilliseconds(3000));
    }
}
