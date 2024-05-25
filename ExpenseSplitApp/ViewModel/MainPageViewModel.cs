using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExpenseSplitApp.Models;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ExpenseSplitApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Group> _groups;
        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddGroupCommand { get; }
        public ICommand DeleteGroupCommand { get; }
        public ICommand EditGroupCommand { get; }

        public MainPageViewModel()
        {
            AddGroupCommand = new Command(async () => await AddGroupAsync());
            DeleteGroupCommand = new Command<Group>(async (group) => await DeleteGroupAsync(group));
            EditGroupCommand = new Command<Group>(async (group) => await EditGroupAsync(group));
            LoadGroups();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadGroups()
        {
            var groups = App.Database.GetGroupsAsync().Result;
            Groups = new ObservableCollection<Group>(groups);
        }

        public async Task AddGroupAsync()
        {
            string groupName = await App.Current.MainPage.DisplayPromptAsync("Nowa Grupa", "Podaj nazwę grupy:");
            if (!string.IsNullOrWhiteSpace(groupName))
            {
                var newGroup = new Group { Name = groupName };
                await App.Database.SaveGroupAsync(newGroup);
                Groups.Add(newGroup);
            }
        }

        public async Task DeleteGroupAsync(Group group)
        {
            if (group != null)
            {
                await App.Database.DeleteParticipantsByGroupIdAsync(group.Id);
                await App.Database.DeleteExpensesByGroupIdAsync(group.Id);
                await App.Database.DeleteGroupAsync(group);
                Groups.Remove(group);
            }
        }

        public async Task EditGroupAsync(Group group)
        {
            if (group != null)
            {
                string newName = await App.Current.MainPage.DisplayPromptAsync("Edytuj Grupę", "Podaj nową nazwę grupy:", initialValue: group.Name);
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    group.Name = newName;
                    await App.Database.UpdateGroupAsync(group);
                    LoadGroups();  // Reload groups to reflect changes
                }
            }
        }
    }
}

