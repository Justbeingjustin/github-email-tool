using Github.EmailAddressTool.Donations;
using Github.EmailAddressTool.EmailProviders;
using Prism.Commands;
using Prism.Mvvm;

namespace Github.EmailAddressTool
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand WindowLoaded { get; private set; }
        public DelegateCommand<string> strategyCommand { get; private set; }
        public DelegateCommand commandToggleDrawer { get; private set; }

        public const string emailPage = "EmailProvider";
        public const string donatePage = "Donate";

        private BindableBase _CurrentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value); }
        }

        private bool _showLeftHamburgerMenu;
        public bool ShowLeftHamburgerMenu
        {
            get { return _showLeftHamburgerMenu; }
            set { SetProperty(ref _showLeftHamburgerMenu, value); }
        }

        private bool _shouldLeftDrawerBeOpen;
        public bool ShouldLeftDrawerBeOpen
        {
            get { return _shouldLeftDrawerBeOpen; }
            set { SetProperty(ref _shouldLeftDrawerBeOpen, value); }
        }

        private readonly IEmailProviderViewModel _emailProvider;
        private readonly IDonationsViewModel _donationsViewModel;


        public MainWindowViewModel(IEmailProviderViewModel emailProvider, IDonationsViewModel donationsViewModel )
        {
            _emailProvider = emailProvider;
            _donationsViewModel = donationsViewModel;

            WindowLoaded = new DelegateCommand(Load);
            strategyCommand = new DelegateCommand<string>(Navigate);
            commandToggleDrawer = new DelegateCommand(ToggleDrawer);
            ShowLeftHamburgerMenu = true;
        }

        private void Load()
        {
            Navigate(emailPage);
        }


        private void ToggleDrawer()
        {
            this.ShouldLeftDrawerBeOpen = !this.ShouldLeftDrawerBeOpen;
        }

        private void Navigate(string pageName)
        {
            ShouldLeftDrawerBeOpen = false;

            if (pageName == donatePage)
            {
                this.CurrentViewModel = (BindableBase)_donationsViewModel;
            }
            else if (pageName == emailPage) {
                this.CurrentViewModel = (BindableBase)_emailProvider;
            }

            
        }
    }
}
