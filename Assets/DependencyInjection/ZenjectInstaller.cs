using effect;
using navigation;
using resource;
using rhythm;
using shop;
using Zenject;

namespace DependencyInjection
{
    public class ZenjectInstaller : MonoInstaller
    {
        public RhythmController rhythmController;
        public ResourceController resourceController;
        public AudioController audioController;
        public NavigationController navigationController;
        public ShopController shopController;
        public EffectController effectController;

        public NavigationItem navigationItemPrefab;
        public ShopOfferUI shopOfferPrefab;

        public override void InstallBindings()
        {
            Container.Bind<RhythmController>().FromInstance(rhythmController);
            Container.Bind<ResourceController>().FromInstance(resourceController);
            Container.Bind<AudioController>().FromInstance(audioController);
            Container.Bind<NavigationController>().FromInstance(navigationController);
            Container.Bind<ShopController>().FromInstance(shopController);
            Container.Bind<EffectController>().FromInstance(effectController);

            Container.BindFactory<NavigationItem, NavigationItemFactory>()
                .FromComponentInNewPrefab(navigationItemPrefab);
            Container.BindFactory<ShopOfferUI, ShopOfferFactory>()
                .FromComponentInNewPrefab(shopOfferPrefab);
        }
    }
}