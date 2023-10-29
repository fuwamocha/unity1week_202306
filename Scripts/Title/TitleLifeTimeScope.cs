using Cysharp.Threading.Tasks;
using Game.Scripts.Title.Credit;
using MochaLib.Audio;
using MochaLib.Credit;
using MochaLib.Settings;
using VContainer;
using VContainer.Unity;
namespace Game.Scripts.Title
{
    public class TitleLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<StartView>();
            builder.RegisterEntryPoint<StartPresenter>();

            builder.RegisterComponentInHierarchy<TitleView>();
            builder.RegisterEntryPoint<TitlePresenter>();

            builder.RegisterComponentInHierarchy<CreditView>();
            builder.RegisterEntryPoint<CreditPresenter>();

            builder.RegisterComponentInHierarchy<CreatorView>();
            builder.RegisterEntryPoint<CreatorPresenter>();

            builder.RegisterBuildCallback(resolver =>
            {
                resolver.Resolve<AudioPlayer>().PlayBgm(CommonConstants.Audio.Bgm.Title).Forget();
            });
        }
    }
}
