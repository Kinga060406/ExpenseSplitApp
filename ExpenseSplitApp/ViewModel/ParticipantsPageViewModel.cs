using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ExpenseSplitApp.Models;

namespace ExpenseSplitApp.ViewModels
{
    public class ParticipantsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Participant> _participants;
        private ObservableCollection<Expense> _expenses;
        private int _groupId;

        public ObservableCollection<Participant> Participants
        {
            get => _participants;
            set
            {
                _participants = value;
                OnPropertyChanged(nameof(Participants));
            }
        }

        public ObservableCollection<Expense> Expenses
        {
            get => _expenses;
            set
            {
                _expenses = value;
                OnPropertyChanged(nameof(Expenses));
            }
        }

        public ParticipantsViewModel(int groupId)
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

        public async Task AddParticipantAsync(string participantName)
        {
            if (!string.IsNullOrWhiteSpace(participantName))
            {
                var newParticipant = new Participant { Name = participantName, GroupId = _groupId };
                await App.Database.SaveParticipantAsync(newParticipant);
                Participants.Add(newParticipant);
            }
        }

        public async Task AddExpenseAsync(string description, string amountString)
        {
            if (!string.IsNullOrWhiteSpace(description) && decimal.TryParse(amountString, out var amount))
            {
                var newExpense = new Expense { Description = description, Amount = amount, GroupId = _groupId };
                await App.Database.SaveExpenseAsync(newExpense);
                Expenses.Add(newExpense);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
