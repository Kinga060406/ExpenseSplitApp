// MainPage.xaml.cs
using ExpenseSplitApp.Models;
using System.Collections.ObjectModel;
using ExpenseSplitApp.View;

namespace ExpenseSplitApp
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Group> _groups;

        public MainPage()
        {
            InitializeComponent();
            LoadGroups();
        }

        private async void LoadGroups()
        {
            var groups = await App.Database.GetGroupsAsync();
            _groups = new ObservableCollection<Group>(groups);
            groupsListView.ItemsSource = _groups;
        }

        private async void OnAddGroupClicked(object sender, EventArgs e)
        {
            var groupName = await DisplayPromptAsync("Nowa Grupa", "Podaj nazwę grupy:");
            if (!string.IsNullOrWhiteSpace(groupName))
            {
                var newGroup = new Group { Name = groupName };
                await App.Database.SaveGroupAsync(newGroup);
                _groups.Add(newGroup);
            }
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