using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ExpenseSplitApp.Models;

namespace ExpenseSplitApp.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private string _confirmPassword;

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

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RegisterViewModel()
        {
            RegisterCommand = new Command(async () => await RegisterAsync());
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task RegisterAsync()
        {
            if (Password != ConfirmPassword)
            {
                await App.Current.MainPage.DisplayAlert("Błąd", "Hasła się nie zgadzają", "OK");
                return;
            }

            var existingUser = await App.Database.GetUserByUsernameAsync(Username);
            if (existingUser != null)
            {
                await App.Current.MainPage.DisplayAlert("Błąd", "Użytkownik o tej nazwie już istnieje", "OK");
                return;
            }

            var newUser = new User { Username = Username, Password = Password }; // W praktyce hasła powinny być haszowane
            await App.Database.SaveUserAsync(newUser);
            await App.Current.MainPage.DisplayAlert("Sukces", "Rejestracja zakończona pomyślnie", "OK");
            await App.Current.MainPage.Navigation.PushAsync(new MainPage(newUser.Id)); // Nawiguj do MainPage z UserId
        }
    }
}
