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
            await CalculateDebtsAsync();
        }

        private async void LoadExpenses()
        {
            var expenses = await App.Database.GetExpensesAsync();
            Expenses = new ObservableCollection<Expense>(expenses.Where(e => e.GroupId == _groupId));
            await CalculateDebtsAsync();
        }

        private async Task CalculateDebtsAsync()
        {
            var participants = await App.Database.GetParticipantsAsync();
            var expenses = await App.Database.GetExpensesAsync();
            await DebtCalculator.CalculateDebtsAsync(participants, expenses, App.Database);

            var updatedParticipants = await App.Database.GetParticipantsAsync();
            Participants = new ObservableCollection<Participant>(updatedParticipants.Where(p => p.GroupId == _groupId));
            OnPropertyChanged(nameof(Participants));
        }

        public async Task AddParticipantAsync(string participantName)
        {
            if (!string.IsNullOrWhiteSpace(participantName))
            {
                var newParticipant = new Participant { Name = participantName, GroupId = _groupId };
                await App.Database.SaveParticipantAsync(newParticipant);
                Participants.Add(newParticipant);
                await CalculateDebtsAsync();
            }
        }

        public async Task AddExpenseAsync(string description, string amountString)
        {
            if (!string.IsNullOrWhiteSpace(description) && decimal.TryParse(amountString, out var amount))
            {
                var newExpense = new Expense { Description = description, Amount = amount, GroupId = _groupId };
                await App.Database.SaveExpenseAsync(newExpense);
                Expenses.Add(newExpense);
                await CalculateDebtsAsync();
            }
        }

        public async Task EditParticipantAsync(Participant participant, string newName)
        {
            if (participant != null && !string.IsNullOrWhiteSpace(newName))
            {
                participant.Name = newName;
                await App.Database.UpdateParticipantAsync(participant);
                LoadParticipants();
            }
        }

        public async Task EditExpenseAsync(Expense expense, string newDescription, string newAmountString)
        {
            if (expense != null && !string.IsNullOrWhiteSpace(newDescription) && decimal.TryParse(newAmountString, out var newAmount))
            {
                expense.Description = newDescription;
                expense.Amount = newAmount;
                await App.Database.SaveExpenseAsync(expense);
                LoadExpenses();
            }
        }

        public async Task DeleteParticipantAsync(Participant participant)
        {
            if (participant != null)
            {
                await App.Database.DeleteParticipantAsync(participant);
                Participants.Remove(participant);
                await CalculateDebtsAsync();
            }
        }

        public async Task DeleteExpenseAsync(Expense expense)
        {
            if (expense != null)
            {
                await App.Database.DeleteExpenseAsync(expense);
                Expenses.Remove(expense);
                await CalculateDebtsAsync();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
