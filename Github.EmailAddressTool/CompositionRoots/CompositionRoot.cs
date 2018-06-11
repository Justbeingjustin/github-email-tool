using Github.EmailAddressTool.Donations;
using Github.EmailAddressTool.EmailProviders;
using Github.Services;
using Unity;

namespace Github.EmailAddressTool.CompositionRoots
{
    public static class CompositionRoot
    {
        public static IUnityContainer Configure()
        {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IGithubUserRepositoryFactory, GithubUserRepositoryFactory>();
            unityContainer.RegisterType<IEmailProviderViewModel, EmailProviderViewModel>();
            unityContainer.RegisterType<IDonationsViewModel, DonationsViewModel>();
            return unityContainer;
        }

    }
}
