using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Scripts.InGame.Ordered;
using Game.Scripts.InGame.Toppings;
using UniRx;
using UnityEngine;
using VContainer.Unity;
namespace Game.Scripts.InGame.SupplyWaffle
{
    public class WaffleSupplyModel : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private readonly bool[] _isToppingOn = new bool[Enum.GetValues(typeof(ToppingType)).Length];
        public ReactiveProperty<Unit> NotExist { get; } = new();
        public ReactiveProperty<Unit> Earned { get; } = new();
        public void Dispose() => _disposable.Dispose();

        public void Initialize() { throw new NotImplementedException(); }

        public void ResetToppings()
        {
            for (int i = 0; i < _isToppingOn.Length; i++)
            {
                _isToppingOn[i] = false;
            }
        }

        public void InGame()
        {
            _isToppingOn.Initialize();
        }

        private void PutTopping(ToppingType toppingType)
        {
            _isToppingOn[(int)toppingType] = true;
#if UNITY_EDITOR
            Debug.Log($"Put {toppingType} on waffle");
#endif
        }
        public async UniTask PutOn(int index)
        {
            switch (index)
            {
                case 0:
                    await UniTask.Delay(200);
                    PutTopping(ToppingType.Kiwi);
                    break;
                case 1:
                    await UniTask.Delay(200);
                    PutTopping(ToppingType.Vanilla);
                    break;
                case 2:
                    await UniTask.Delay(200);
                    PutTopping(ToppingType.Banana);
                    break;
                case 3:
                    await UniTask.Delay(200);
                    PutTopping(ToppingType.Strawberry);
                    break;
            }
        }

        public async UniTask Supply(List<OrderedWaffle> orderedWafflePresenters)
        {
            await UniTask.Delay(200);
            CheckToppings(orderedWafflePresenters);
        }
        private void CheckToppings(List<OrderedWaffle> orderedWafflePresenters)
        {
            if (orderedWafflePresenters is null) return;
#if UNITY_EDITOR
            Debug.Log("topping on: \n" + string.Join(", ", Enum.GetNames(typeof(ToppingType))) + "\n" + string.Join(", ", _isToppingOn.Select(t => t.ToString()).ToArray()));
#endif
            for (int i = 0; i < orderedWafflePresenters.Count; i++)
            {
                var waffleOrder = orderedWafflePresenters[i];
#if UNITY_EDITOR
                Debug.Log("correct topping: " + string.Join(", ", Enum.GetNames(typeof(ToppingType))) + "\n" + string.Join(", ", waffleOrder.Toppings.Select(t => t.ToString()).ToArray()));
#endif
                if (waffleOrder.Toppings.SequenceEqual(_isToppingOn))
                {
#if UNITY_EDITOR
                    Debug.Log("Correct Topping");
#endif
                    waffleOrder.Success();
                    Earned.SetValueAndForceNotify(Unit.Default);
                    return;
                }
#if UNITY_EDITOR
                Debug.Log("Wrong Topping");
#endif
            }
            NotExist.SetValueAndForceNotify(Unit.Default);
        }
    }
}
