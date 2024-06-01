using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ExpenseSplitApp.Models;

namespace ExpenseSplitApp.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _email;
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

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
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

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Użyj prostego regexa do walidacji emaila
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return emailRegex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await App.Current.MainPage.DisplayAlert("Błąd", "Wszystkie pola są wymagane.", "OK");
                return;
            }

            if (!IsValidEmail(Email))
            {
                await App.Current.MainPage.DisplayAlert("Błąd", "Nieprawidłowy adres e-mail.", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await App.Current.MainPage.DisplayAlert("Błąd", "Hasła się nie zgadzają.", "OK");
                return;
            }

            var existingUser = await App.Database.GetUserByUsernameAsync(Username);
            if (existingUser != null)
            {
                await App.Current.MainPage.DisplayAlert("Błąd", "Użytkownik o tej nazwie już istnieje.", "OK");
                return;
            }

            var newUser = new User { Username = Username, Password = Password, Email = Email };
            await App.Database.SaveUserAsync(newUser);
            await App.Current.MainPage.DisplayAlert("Sukces", "Rejestracja zakończona pomyślnie.", "OK");
            await App.Current.MainPage.Navigation.PushAsync(new MainPage(newUser.Id)); // Nawiguj do MainPage z UserId
        }
    }
}
