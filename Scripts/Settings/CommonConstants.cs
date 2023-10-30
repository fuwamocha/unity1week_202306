using Game.Scripts.InGame.Toppings;
using UnityEngine;
namespace MochaLib.Settings
{
    public static class CommonConstants
    {
        public static class InGame
        {
            public const int MinOrderDelay = 1000;
            public const int MaxOrderDelay = 3000;
            public const int MaxOrderCount = 10;
        }
        public static class Waffle
        {
            public const float TimeLimit = 13f;
            public static class Animation
            {
                public static readonly int DeleteOrder = Animator.StringToHash("Order");
                public static readonly int SuccessOrder = Animator.StringToHash("OrderSuccess");
            }
        }
        public static class Character
        {
            public static class Animation
            {
                private static readonly int PutBanana = Animator.StringToHash("banana");
                private static readonly int PutStrawberry = Animator.StringToHash("strawberry");
                private static readonly int PutKiwi = Animator.StringToHash("kiwi");
                private static readonly int PutVanilla = Animator.StringToHash("ice");
                public static readonly int OutTentacles = Animator.StringToHash("shokusyu_out");
                public static readonly int NoTentacles = Animator.StringToHash("no_shokusyu");
                public static readonly int Success = Animator.StringToHash("smile");
                public static readonly int Miss = Animator.StringToHash("miss");
                public static readonly int Sigh = Animator.StringToHash("tameiki");
                public static readonly int JyanAge = Animator.StringToHash("jyan_age");
                public static readonly int JyanOroshi = Animator.StringToHash("jyan_oroshi");
                public static int Top(ToppingType toppingType)
                {
                    return toppingType switch
                    {
                        ToppingType.Banana     => PutBanana,
                        ToppingType.Strawberry => PutStrawberry,
                        ToppingType.Kiwi       => PutKiwi,
                        ToppingType.Vanilla    => PutVanilla,
                        _                      => -1
                    };
                }
            }
        }
        public static class Audio
        {
            public static class Bgm
            {
                public const float InitVolume = 0.3f;
                public const string Title = "title";
                public const string InGame = "in_game";
            }
            public static class Se
            {
                public const float InitVolume = 0.5f;
                public const string Click = "click";
                public const string Topping = "topping";
                public const string Supply = "supply";
                public const string Miss = "miss";
                public const string StopPlaying = "stop_playing";
                public const string Don = "don";
                public const string Dodon = "dodon";
                public const string Flip = "flip";
                public const string Earned = "earned";
            }
        }
    }
}
