using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ExpenseSplitApp.Models;

namespace ExpenseSplitApp.ViewModels
{
    public class ParticipantsPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Participant> _participants;
        public ObservableCollection<Participant> Participants
        {
            get => _participants;
            set
            {
                _participants = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Expense> _expenses;
        public ObservableCollection<Expense> Expenses
        {
            get => _expenses;
            set
            {
                _expenses = value;
                OnPropertyChanged();
            }
        }

        private int _groupId;

        public ParticipantsPageViewModel(int groupId)
        {
            _groupId = groupId;
            LoadParticipants();
            LoadExpenses();
        }

        private async void LoadParticipants()
        {
            var participants = await App.Database.GetParticipantsAsync();
            Participants = new ObservableCollection<Participant>(participants.Where(p => p.GroupId == _groupId));
        }

        private async void LoadExpenses()
        {
            var expenses = await App.Database.GetExpensesAsync();
            Expenses = new ObservableCollection<Expense>(expenses.Where(e => e.GroupId == _groupId));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
