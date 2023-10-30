using System;
using UniRx;
using UnityEngine;
namespace MochaLib.InGame.CountDown
{
    /// <summary>
    ///     残り時間タイマー
    /// </summary>
    public class RemainingTimer
    {
        private const float ElapsedTime = 0;
        private readonly ISubject<Unit> _onFinish = new Subject<Unit>();

        private readonly ReactiveProperty<float> _remainingTime = new(0f);

        private TimeSpan _timeLimit;

        public IReadOnlyReactiveProperty<float> RemainingTime => _remainingTime;
        public IObservable<Unit> OnFinish => _onFinish;

        public TimeSpan TimeLimit => _timeLimit;

        /// <summary>
        ///     ゲームの制限時間を設定する
        /// </summary>
        public void SetTime(TimeSpan timeSpan)
        {
#if UNITY_EDITOR
            Debug.Log("timer set, " + timeSpan);
#endif
            _timeLimit = timeSpan;
            _remainingTime.Value = (float)_timeLimit.TotalSeconds - ElapsedTime;
        }

        public void SetElapsedTime(float elapsedTime)
        {
            _remainingTime.Value = Mathf.Max(0f, (float)_timeLimit.TotalSeconds - elapsedTime);
            if (Mathf.Approximately(_remainingTime.Value, 0f))
            {
                _onFinish.OnNext(Unit.Default);
            }
        }

        public bool IsFinish() => _remainingTime.Value == 0f;
    }
}
