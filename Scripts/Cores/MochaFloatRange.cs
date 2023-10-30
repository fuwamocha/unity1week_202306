using System;
using UnityEngine;
namespace MochaLib.Cores
{
    [Serializable]
    public struct MochaFloatRange : IRange<float>
    {
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;

        public MochaFloatRange(float min, float max)
        {
            _minValue = min;
            _maxValue = Mathf.Max(min, max);
        }

        public float Min
        {
            get => _minValue;
            set => _minValue = Mathf.Min(value, _maxValue);
        }

        public float Max
        {
            get => _maxValue;
            set => _maxValue = Mathf.Max(value, _minValue);
        }

        public float Mid => _minValue + (_maxValue - _minValue) / 2;
        public float Random => _minValue < _maxValue ? UnityEngine.Random.Range(_minValue, _maxValue + 1) : _minValue;
    }
}
