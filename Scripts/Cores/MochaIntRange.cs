using System;
using UnityEngine;
namespace MochaLib.Cores
{
    [Serializable]
    public struct MochaIntRange : IRange<int>
    {
        [SerializeField] private int _minValue;
        [SerializeField] private int _maxValue;

        public MochaIntRange(int min, int max)
        {
            _minValue = min;
            _maxValue = Mathf.Max(min, max);
        }

        public int Min
        {
            get => _minValue;
            set => _minValue = Mathf.Min(value, _maxValue);
        }

        public int Max
        {
            get => _maxValue;
            set => _maxValue = Mathf.Max(value, _minValue);
        }

        public int Mid => _minValue + (_maxValue - _minValue) / 2;
        public int Random => _minValue < _maxValue ? UnityEngine.Random.Range(_minValue, _maxValue + 1) : _minValue;
    }
}
