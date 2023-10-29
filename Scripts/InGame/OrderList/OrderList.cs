using System.Collections.Generic;
using Game.Scripts.InGame.Ordered;
using MochaLib.Settings;
namespace Game.Scripts.InGame.OrderList
{
    public class OrderList
    {
        public readonly List<OrderedWaffle> OrderedWaffles = new(CommonConstants.InGame.MaxOrderCount);


        public void RemoveOrder(OrderedWaffle orderedWaffle)
        {
            OrderedWaffles.Remove(orderedWaffle);
        }

        public void AddOrder(OrderedWaffle orderedWaffle)
        {
            OrderedWaffles.Add(orderedWaffle);
        }

        public void End()
        {
            ResetOrderObjectAll(OrderedWaffles);
            OrderedWaffles.Clear();
        }

        private static void ResetOrderObjectAll(List<OrderedWaffle> orderedWaffles)
        {
            foreach (var orderedWaffle in orderedWaffles)
            {
                orderedWaffle.DestroyOrder();
            }
        }
    }
}
