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

        private async void OnDeleteAccountClicked(object sender, EventArgs e)
        {
            bool confirmed = await DisplayAlert("Usuń konto", "Czy na pewno chcesz usunąć swoje konto? Wszystkie dane zostaną utracone.", "Tak", "Nie");
            if (confirmed)
            {
                // Usuń wszystkie dane użytkownika
                var groups = await App.Database.GetGroupsAsync(viewModel.CurrentUserId);
                foreach (var group in groups)
                {
                    await App.Database.DeleteParticipantsByGroupIdAsync(group.Id);
                    await App.Database.DeleteExpensesByGroupIdAsync(group.Id);
                    await App.Database.DeleteGroupAsync(group);
                }

                var user = await App.Database.GetUserByIdAsync(viewModel.CurrentUserId);
                if (user != null)
                {
                    await App.Database.DeleteUserAsync(user);
                }

                // Przeniesienie do strony logowania
                await Navigation.PushAsync(new LoginPage());
                // Usuń bieżącą stronę z historii nawigacji
                Navigation.RemovePage(this);
            }
        }
    }
}
