using ExpenseSplitApp.Models;
using System;
using System.Collections.ObjectModel;
using ExpenseSplitApp.View;
using ExpenseSplitApp.ViewModels;

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
            viewModel.LoadGroups();
        }


        private async void OnAddGroupClicked(object sender, EventArgs e)
        {
            string groupName = await DisplayPromptAsync("Nowa Grupa", "Podaj nazwę grupy:");
            viewModel.AddGroup(groupName);
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
