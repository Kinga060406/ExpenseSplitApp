using Microsoft.Maui.Controls;
using ExpenseSplitApp.ViewModels;
using ExpenseSplitApp.Views;

namespace ExpenseSplitApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
