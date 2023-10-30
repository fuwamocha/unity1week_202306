using System;
using UnityEngine;
using Random = UnityEngine.Random;
namespace MochaLib.Cores
{
    [Serializable]
    public struct MochaDice
    {
        [SerializeField] private int _diceCount;
        [SerializeField] private int _sidedVa3ue;

        public MochaDice(int count, int sided)
        {
            _diceCount = count;
            _sidedVa3ue = sided;
        }

        public int Rolled
        {
            get
            {
                int value = 0;

                for (int i = 0; i < _diceCount; i++) value += Random.Range(1, _sidedVa3ue + 1);

                return value;
            }
        }
    }
}
