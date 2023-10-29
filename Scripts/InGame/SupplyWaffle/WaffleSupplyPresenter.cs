using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.InGame.Toppings;
using MochaLib.Audio;
using MochaLib.InGame;
using MochaLib.Settings;
using UniRx;
using UnityEngine;
using VContainer.Unity;
namespace Game.Scripts.InGame.SupplyWaffle
{
    public class WaffleSupplyPresenter : IInitializable, IDisposable
    {
        private readonly AudioPlayer _audioPlayer;
        private readonly Character.Character _character;
        private readonly CompositeDisposable _disposable = new();
        private readonly InGameStateUseCase _inGameStateUseCase;
        private readonly OrderList.OrderList _orderList;
        private readonly SuppliedWaffleView _suppliedWaffleView;
        private readonly WaffleSupplyModel _waffleSupplyModel;

        public WaffleSupplyPresenter(
            OrderList.OrderList orderList,
            InGameStateUseCase inGameStateUseCase,
            SuppliedWaffleView suppliedWaffleView,
            WaffleSupplyModel waffleSupplyModel,
            AudioPlayer audioPlayer, Character.Character character
        )
        {
            _orderList = orderList;
            _inGameStateUseCase = inGameStateUseCase;
            _waffleSupplyModel = waffleSupplyModel;
            _suppliedWaffleView = suppliedWaffleView;
            _audioPlayer = audioPlayer;
            _character = character;
        }

        public void Dispose() => _disposable.Dispose();

        public void Initialize()
        {


            _waffleSupplyModel.Earned.Skip(1).Subscribe(_ =>
            {
                _audioPlayer.PlaySe(CommonConstants.Audio.Se.Earned);
                _waffleSupplyModel.ResetToppings();
            });
            _waffleSupplyModel.NotExist.Skip(1).Subscribe(_ =>
            {
                _audioPlayer.PlaySe(CommonConstants.Audio.Se.Miss);
                _character.Miss();
                _waffleSupplyModel.ResetToppings();
            });
        }
        private void PutTopping(ToppingType toppingType)
        {
            _character.Top(toppingType);
            _suppliedWaffleView.ShowTopping(toppingType);
        }
        public void End()
        {
            _suppliedWaffleView.HideToppingAll();
            _waffleSupplyModel.ResetToppings();
            _suppliedWaffleView.ChangeActiveButtons(false);
        }

        public void Start()
        {
            _waffleSupplyModel.InGame();
            _suppliedWaffleView.ChangeActiveButtons(true);
        }
    }
}
