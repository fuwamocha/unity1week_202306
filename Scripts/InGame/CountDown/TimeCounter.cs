using UniRx;
using UnityEngine;
using VContainer.Unity;
namespace MochaLib.InGame.CountDown
{
    /// <summary>
    ///     経過時間のカウント
    /// </summary>
    public class TimeCounter : ITickable
    {
        private readonly ReactiveProperty<float> _elapsedTime = new();
        private bool _isRun;
        private float _startTime;
        public IReadOnlyReactiveProperty<float> ElapsedTime => _elapsedTime;

        void ITickable.Tick()
        {
            if (!_isRun) return;
            _elapsedTime.Value = Time.time - _startTime;
        }

        public void Start()
        {
            _startTime = Time.time;
            _elapsedTime.Value = 0f;
            _isRun = true;
        }

        public void Stop()
        {
            _isRun = false;
        }

        public void Reset()
        {
            _elapsedTime.Value = 0;
        }
    }
}
