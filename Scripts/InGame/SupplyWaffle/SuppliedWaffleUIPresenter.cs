using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.InGame.Toppings;
using MochaLib.Audio;
using MochaLib.InGame;
using MochaLib.Settings;
using UniRx;
using UnityEngine;
using VContainer.Unity;
namespace Game.Scripts.InGame.SupplyWaffle.UI
{
    public class SuppliedWaffleUIPresenter : IInitializable, IDisposable
    {
        private readonly Character.Character _character;
        private readonly CompositeDisposable _disposable = new();
        private readonly InGameStateUseCase _inGameStateUseCase;
        private readonly SuppliedWaffleView _suppliedWaffleView;
        private readonly ToppingsView _toppingsView;
        private readonly WaffleSupplyModel _waffleSupplyModel;
        private readonly AudioPlayer _audioPlayer;
        private readonly OrderList.OrderList _orderList;

        public SuppliedWaffleUIPresenter(
            SuppliedWaffleView suppliedWaffleView,
            ToppingsView toppingsView,
            WaffleSupplyModel waffleSupplyModel,
            InGameStateUseCase inGameStateUseCase,
            Character.Character character,
            AudioPlayer audioPlayer,
            OrderList.OrderList orderList
        )
        {
            _suppliedWaffleView = suppliedWaffleView;
            _toppingsView = toppingsView;
            _waffleSupplyModel = waffleSupplyModel;
            _inGameStateUseCase = inGameStateUseCase;
            _character = character;
            _audioPlayer = audioPlayer;
            _orderList = orderList;
        }

        public void Dispose() => _disposable.Dispose();
        public void Initialize()
        {
            _waffleSupplyModel.Earned
                .Subscribe(_ => _toppingsView.HideAll())
                .AddTo(_disposable);
            _waffleSupplyModel.NotExist
                .Subscribe(_ => _toppingsView.HideAll())
                .AddTo(_disposable);

            for (int i = 0; i < 4; i++)
            {
                int index = i;
                _toppingsView.OnClickToppingAsObservable(index)
                    .Where(_ => _inGameStateUseCase.IsState(InGameStateType.Playing))
                    .Merge(Observable.EveryUpdate().Where(_ =>
                        {
                            return index switch
                            {
                                0 => Input.GetKeyDown(KeyCode.K),
                                1 => Input.GetKeyDown(KeyCode.J),
                                2 => Input.GetKeyDown(KeyCode.D),
                                3 => Input.GetKeyDown(KeyCode.F),
                                _ => false
                            };
                        }
                        ).AsUnitObservable())
                    .Subscribe(_ =>
                    {
                        _audioPlayer.PlaySe(CommonConstants.Audio.Se.Topping);

                        PutTopping(index);

                        _waffleSupplyModel.PutOn(index).Forget();
                        _toppingsView.AnimateButton((ToppingType)index);
                    })
                    .AddTo(_disposable);
            }
            _suppliedWaffleView.OnClickWaffleAsObservable()
                .Where(_ => _inGameStateUseCase.IsState(InGameStateType.Playing))
                .Merge(Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.Space)
                    ).AsUnitObservable())
                .Subscribe(_ =>
                {
                    _audioPlayer.PlaySe(CommonConstants.Audio.Se.Supply);
                    _waffleSupplyModel.Supply(_orderList.OrderedWaffles).Forget();

                    _suppliedWaffleView.AnimateButton();
                })
                .AddTo(_disposable);
        }
        private void PutTopping(int index)
        {
            _character.Top((ToppingType)index);
            _toppingsView.Show(index);
        }
        public void InitializeUIBindings(IObservable<Unit> onToppingClicked, int index)
        {
            onToppingClicked.Subscribe(_ =>
            {
                _toppingsView.AnimateButton((ToppingType)index);
            }).AddTo(_disposable);
        }

        public void ShowSupplySet()
        {
            _suppliedWaffleView.Show();
            _toppingsView.ShowAll();
        }

        public void HideSupplySet()
        {
            _suppliedWaffleView.Hide();
            _toppingsView.HideAll();
        }

        public void ToggleActiveButtons(bool isActive)
        {
            _suppliedWaffleView.ChangeWaffleActivate(isActive);
            _toppingsView.ChangeToppingsActivate(isActive);
        }
    }
}
