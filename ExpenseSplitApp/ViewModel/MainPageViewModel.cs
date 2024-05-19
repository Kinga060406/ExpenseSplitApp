﻿using ExpenseSplitApp.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

        public MainPageViewModel()
        {
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

        public async void AddGroup(string groupName)
        {
            if (!string.IsNullOrWhiteSpace(groupName))
            {
                var newGroup = new Group { Name = groupName };
                await App.Database.SaveGroupAsync(newGroup);
                Groups.Add(newGroup);
            }
        }


    }
}
