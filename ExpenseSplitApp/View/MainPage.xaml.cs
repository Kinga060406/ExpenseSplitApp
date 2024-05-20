using Microsoft.Maui.Controls;
using ExpenseSplitApp.ViewModels;
using ExpenseSplitApp.Models; 
using ExpenseSplitApp.Views;

namespace ExpenseSplitApp
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainPageViewModel();
            BindingContext = viewModel;
        }

        private async void OnGroupTapped(object sender, ItemTappedEventArgs e)
        {
            var group = e.Item as Group;
            if (group != null)
            {
                await Navigation.PushAsync(new ParticipantsPage(group.Id));
            }
        }
    }
}
