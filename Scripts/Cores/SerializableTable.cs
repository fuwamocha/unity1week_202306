using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace MochaLib.Cores
{
    /// <summary>
    ///     テーブルの管理クラス
    /// </summary>
    [Serializable]
    public class TableBase<TKey, TValue, Type> where Type : KeyAndValue<TKey, TValue>
    {
        [SerializeField]
        private List<Type> _list;
        private Dictionary<TKey, TValue> _table;
        public int Length => _list.Count;

        public Dictionary<TKey, TValue> GetTable()
        {
            return _table ??= ToDictionary(_list);
        }

        private static Dictionary<TKey, TValue> ToDictionary(IEnumerable<Type> list)
        {
            return list.ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }

    /// <summary>
    ///     シリアル化できる、KeyValuePair
    /// </summary>
    [Serializable]
    public class KeyAndValue<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public KeyAndValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
        public KeyAndValue(KeyValuePair<TKey, TValue> pair)
        {
            Key = pair.Key;
            Value = pair.Value;
        }
    }
}
