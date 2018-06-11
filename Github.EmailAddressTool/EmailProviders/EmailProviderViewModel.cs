using Github.EmailAddressTool.Dialogs;
using Github.Services;
using Github_Email_Finder;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Github.EmailAddressTool.EmailProviders
{
    public class EmailProviderViewModel : BindableBase, IEmailProviderViewModel
    {
        public DelegateCommand WindowLoaded { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }
        public DelegateCommand<PasswordBox> PasswordChangedCommand { get; private set; }


        private string _searchUsername;
        public string SearchUsername
        {
            get { return _searchUsername; }
            set { SetProperty(ref _searchUsername, value); }
        }


        private string _username;
        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }


        private ObservableCollection<GithubUserWithImage> _accounts = new ObservableCollection<GithubUserWithImage>();
        public ObservableCollection<GithubUserWithImage> Accounts
        {
            get { return _accounts; }
            set { SetProperty(ref _accounts, value); }
        }


        private readonly IGithubUserRepositoryFactory _githubUserRepositoryFactory;

        public EmailProviderViewModel(IGithubUserRepositoryFactory githubUserRepositoryFactory)
        {
            _githubUserRepositoryFactory = githubUserRepositoryFactory;
            WindowLoaded = new DelegateCommand(Load);
            SearchCommand = new DelegateCommand(Search);
            PasswordChangedCommand = new DelegateCommand<PasswordBox>(PasswordChanged);
        }

        private void PasswordChanged(PasswordBox passwordBox)
        {
            this.Password = passwordBox.Password;
          
        }

        private async void Search()
        {
            await DialogHost.Show(new LoadingCircle(), DialogHelpers.RootDialog, OnOpen);
        }

        private async void OnOpen(object sender, DialogOpenedEventArgs eventArgs)
        {


            await Task.Run(async () =>
            {
                try
                {
                    var accounts = await _githubUserRepositoryFactory.Create(Username, Password)
                        .GetGithubUsersByUserName(SearchUsername);
                    Accounts = new ObservableCollection<GithubUserWithImage>(accounts);
                }
                catch (Exception ex)
                {
                    Accounts = null;
                }
            });
           
         
           
         
         
            eventArgs.Session.Close(false);
        }

        private void SearchDialogOpened(object sender, DialogClosingEventArgs eventArgs)
        {

        }

        private void Load()
        {

        }
    }
}
