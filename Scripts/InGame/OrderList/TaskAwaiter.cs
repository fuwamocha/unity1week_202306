using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.InGame.Ordered;
using MochaLib.Settings;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Game.Scripts.InGame.OrderList
{
    public class TaskAwaiter
    {
        private CancellationTokenSource _cancellationToken;

        public async UniTask WaitForConditionOrTime(List<OrderedWaffle> orderedWaffles)
        {
            _cancellationToken = new CancellationTokenSource();

            var delayTask = UniTask.Delay(Random.Range(CommonConstants.InGame.MinOrderDelay, CommonConstants.InGame.MaxOrderDelay), cancellationToken: _cancellationToken.Token);
            var conditionTask = WaitUntilZero(orderedWaffles, _cancellationToken.Token);

            await UniTask.WhenAny(delayTask, conditionTask);

            try
            {
                _cancellationToken.Cancel();
            }
            catch (Exception e)
            {
                Debug.Log("canceled");
                throw;
            }
        }

        private static async UniTask WaitUntilZero(ICollection orderedWaffles, CancellationToken token)
        {
            while (orderedWaffles.Count > 0)
            {
                await UniTask.Yield(cancellationToken: token);
            }
        }
    }
}
