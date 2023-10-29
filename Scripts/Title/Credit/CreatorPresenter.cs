using System;
using MochaLib.Audio;
using UniRx;
using UnityEngine;
using VContainer.Unity;
namespace Game.Scripts.Title.Credit
{
    public class CreatorPresenter : IInitializable, IDisposable
    {
        private readonly AudioPlayer _audioPlayer;
        private readonly CreatorView _creatorView;
        private readonly CompositeDisposable _disposable = new();

        public CreatorPresenter(
            CreatorView creatorView,
            AudioPlayer audioPlayer)
        {
            _creatorView = creatorView;
            _audioPlayer = audioPlayer;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public void Initialize()
        {
            _creatorView.OnClickFuwamochaAsObservable()
                .Subscribe(_ =>
                {
                    Application.OpenURL("https://twitter.com/fuwamocha_");
                });
            _creatorView.OnClickRauAsObservable()
                .Subscribe(_ =>
                {
                    Application.OpenURL("https://twitter.com/luge_ast");
                });
        }
    }
}
