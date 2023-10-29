using System;
using Game.Scripts.InGame.Score;
using Game.Scripts.InGame.Toppings;
using MochaLib.InGame.CountDown;
using MochaLib.Settings;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Game.Scripts.InGame.Ordered
{
    public class OrderedWaffle : MonoBehaviour
    {
        private readonly ReactiveProperty<bool> _isSucceeded = new();
        private bool _isRunning;
        private OrderedWaffleView _orderedWaffleView;
        private TimeCounter _timeCounter;

        public IReactiveProperty<bool> IsSucceeded => _isSucceeded;
        public bool[] Toppings { get; private set; }
        public float RemainedTime { get; private set; }

        private void Awake()
        {
            _orderedWaffleView = GetComponent<OrderedWaffleView>();
        }
        private void Update()
        {
            if (!_isRunning) return;

            DecreaseRemainedTime();
            CheckComboTime();

            if (RemainedTime >= 0)
            {
                _orderedWaffleView.UpdateTimerBar(RemainedTime);
            }
            else
            {
                Failed();
            }
        }
        public void OnDestroy()
        {
            _isSucceeded.Dispose();
        }
        public void Initialize()
        {
            SetTimer(CommonConstants.Waffle.TimeLimit);
            StartTimer();
            SetOrder();
        }
        public void DestroyOrder()
        {
            Destroy(gameObject);
        }

        public void Success()
        {
            ComboTimeCounter.ResetTime();
            _isSucceeded.Value = true;

            _orderedWaffleView.SuccessAnimation();
        }
        private void DecreaseRemainedTime()
        {
            RemainedTime -= Time.deltaTime;
        }

        private void CheckComboTime()
        {
            if (ComboTimeCounter.LastSupplyTime >= 5f)
            {
                ScoreManager.ResetCombo();
            }
        }

        private void StartTimer()
        {
            _isRunning = true;
        }
        private void SetTimer(float time)
        {
            RemainedTime = time;
        }

        private void Failed()
        {
            _isRunning = false;
            _isSucceeded.Value = false;

            _orderedWaffleView.FailedAnimation();
        }

        private void SetOrder()
        {
            Toppings = DecideOrder();
            _orderedWaffleView.Set(Toppings);
        }

        private static bool[] DecideOrder()
        {
            bool[] toppings = new bool[Enum.GetValues(typeof(ToppingType)).Length];

            for (int i = 0; i < toppings.Length; i++)
            {
                toppings[i] = Random.Range(0, 2) == 1;
            }
            return toppings;
        }
    }
}
