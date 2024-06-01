using Microsoft.Maui.Controls;
using ExpenseSplitApp.ViewModels;
using ExpenseSplitApp.Models;
using ExpenseSplitApp.Views;

namespace ExpenseSplitApp
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel viewModel;

        public MainPage(int userId)
        {
            InitializeComponent();
            viewModel = new MainPageViewModel(userId);
            BindingContext = viewModel;
        }

        private async void OnGroupTapped(object sender, SelectionChangedEventArgs e)
        {
            var group = e.CurrentSelection.FirstOrDefault() as Group;
            if (group != null)
            {
                await Navigation.PushAsync(new ParticipantsPage(group.Id, viewModel.CurrentUserId));
                // Clear the selection after navigating
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Przeniesienie do strony logowania
            await Navigation.PushAsync(new LoginPage());
            // Usuń bieżącą stronę z historii nawigacji
            Navigation.RemovePage(this);
        }
    }
}
