using ExpenseSplitApp.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseSplitApp.View
{
        public partial class ParticipantsPage : ContentPage
        {
            private ObservableCollection<Participant> _participants;
            private ObservableCollection<Expense> _expenses;
            private int _groupId;

            public ParticipantsPage(int groupId)
            {
                InitializeComponent();
                _groupId = groupId;
                LoadParticipants();
                LoadExpenses();
            }

            private async void LoadParticipants()
            {
                var participants = await App.Database.GetParticipantsAsync();
                _participants = new ObservableCollection<Participant>(participants.Where(p => p.GroupId == _groupId));
                participantsListView.ItemsSource = _participants;
            }

            private async void LoadExpenses()
            {
                var expenses = await App.Database.GetExpensesAsync();
                _expenses = new ObservableCollection<Expense>(expenses.Where(e => e.GroupId == _groupId));
                expensesListView.ItemsSource = _expenses;
            }

            private async void OnAddParticipantClicked(object sender, EventArgs e)
            {
                var participantName = await DisplayPromptAsync("Nowy Uczestnik", "Podaj nazwê uczestnika:");
                if (!string.IsNullOrWhiteSpace(participantName))
                {
                    var newParticipant = new Participant { Name = participantName, GroupId = _groupId };
                    await App.Database.SaveParticipantAsync(newParticipant);
                    _participants.Add(newParticipant);
                }
            }

            private async void OnAddExpenseClicked(object sender, EventArgs e)
            {
                var description = await DisplayPromptAsync("Nowy Wydatek", "Podaj opis wydatku:");
                var amountString = await DisplayPromptAsync("Nowy Wydatek", "Podaj kwotê:");

                if (!string.IsNullOrWhiteSpace(description) && decimal.TryParse(amountString, out var amount))
                {
                    var newExpense = new Expense { Description = description, Amount = amount, GroupId = _groupId };
                    await App.Database.SaveExpenseAsync(newExpense);
                    _expenses.Add(newExpense);
                }
            }
        }
    }
