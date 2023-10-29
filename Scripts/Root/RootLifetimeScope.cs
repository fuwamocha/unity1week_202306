using MochaLib.Audio;
using MochaLib.Audio.Settings;
using MochaLib.InGame;
using MochaLib.Scene;
using MochaLib.Settings;
using UnityEngine;
using VContainer;
using VContainer.Unity;
namespace Game.Scripts.Root
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private AudioResource _audioResource;
        [SerializeField] private SceneTransitionView _transitionViewPrefab;
        protected override void Configure(IContainerBuilder builder)
        {
            AudioInstall(builder);
        }

        private void AudioInstall(IContainerBuilder builder)
        {
            builder.Register(resolver =>
            {
                var resource = Instantiate(_audioResource);
                DontDestroyOnLoad(resource);
                return resource;
            }, Lifetime.Singleton);

            // GameState
            builder.Register<InGameStateUseCase>(Lifetime.Scoped);
            builder.Register<StateUseCase>(Lifetime.Singleton);

            // Audio
            builder.Register<AudioResourceLoader>(Lifetime.Singleton);
            builder.Register<AudioPlayer>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<AudioSettingsService>(Lifetime.Singleton);
            builder.RegisterComponentOnNewGameObject<BgmPlayer>(Lifetime.Singleton).DontDestroyOnLoad();
            builder.RegisterComponentOnNewGameObject<SePlayer>(Lifetime.Singleton).DontDestroyOnLoad();
            builder.RegisterComponentInHierarchy<AudioSettingsView>();
            builder.RegisterEntryPoint<AudioSettingsPresenter>();

            builder.RegisterComponentInNewPrefab(_transitionViewPrefab, Lifetime.Singleton).DontDestroyOnLoad();
        }
    }
}
