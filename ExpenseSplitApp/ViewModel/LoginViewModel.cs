using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ExpenseSplitApp.Models;

namespace ExpenseSplitApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginAsync());
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task LoginAsync()
        {
            var user = await App.Database.GetUserByUsernameAsync(Username);
            if (user != null && user.Password == Password) // W praktyce hasła powinny być sprawdzane za pomocą haszy
            {
                // Zalogowano pomyślnie
                await App.Current.MainPage.DisplayAlert("Sukces", "Zalogowano pomyślnie", "OK");
                await App.Current.MainPage.Navigation.PushAsync(new MainPage(user.Id));
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Błąd", "Nieprawidłowy login lub hasło", "OK");
            }
        }
    }
}
