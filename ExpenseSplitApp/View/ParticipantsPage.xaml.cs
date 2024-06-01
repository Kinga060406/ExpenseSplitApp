using Microsoft.Maui.Controls;
using ExpenseSplitApp.ViewModels;
using ExpenseSplitApp.Models;

namespace ExpenseSplitApp.Views
{
    public partial class ParticipantsPage : ContentPage
    {
        private ParticipantsViewModel _viewModel;

        public ParticipantsPage(int groupId, int userId)
        {
            InitializeComponent();
            _viewModel = new ParticipantsViewModel(groupId, userId);
            BindingContext = _viewModel;
        }

        private async void OnAddParticipantClicked(object sender, EventArgs e)
        {
            var participantName = await DisplayPromptAsync("Nowy Uczestnik", "Podaj nazw� uczestnika:");
            await _viewModel.AddParticipantAsync(participantName);
        }

        private async void OnAddExpenseClicked(object sender, EventArgs e)
        {
            var description = await DisplayPromptAsync("Nowy Wydatek", "Podaj opis wydatku:");
            var amountString = await DisplayPromptAsync("Nowy Wydatek", "Podaj kwot�:");
            await _viewModel.AddExpenseAsync(description, amountString);
        }

        private async void OnEditParticipantClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var participant = button?.CommandParameter as Participant;
            if (participant != null)
            {
                var newName = await DisplayPromptAsync("Edytuj Uczestnika", "Podaj now� nazw� uczestnika:", initialValue: participant.Name);
                await _viewModel.EditParticipantAsync(participant, newName);
            }
        }

        private async void OnEditExpenseClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var expense = button?.CommandParameter as Expense;
            if (expense != null)
            {
                var newDescription = await DisplayPromptAsync("Edytuj Wydatek", "Podaj nowy opis wydatku:", initialValue: expense.Description);
                var newAmountString = await DisplayPromptAsync("Edytuj Wydatek", "Podaj now� kwot�:", initialValue: expense.Amount.ToString());
                await _viewModel.EditExpenseAsync(expense, newDescription, newAmountString);
            }
        }

        private async void OnDeleteParticipantClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var participant = button?.CommandParameter as Participant;
            if (participant != null)
            {
                bool confirmed = await DisplayAlert("Usu� Uczestnika", $"Czy na pewno chcesz usun�� uczestnika {participant.Name}?", "Tak", "Nie");
                if (confirmed)
                {
                    await _viewModel.DeleteParticipantAsync(participant);
                }
            }
        }

        private async void OnDeleteExpenseClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var expense = button?.CommandParameter as Expense;
            if (expense != null)
            {
                bool confirmed = await DisplayAlert("Usu� Wydatek", $"Czy na pewno chcesz usun�� wydatek {expense.Description}?", "Tak", "Nie");
                if (confirmed)
                {
                    await _viewModel.DeleteExpenseAsync(expense);
                }
            }
        }
    }
}
