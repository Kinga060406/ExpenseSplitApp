using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using ExpenseSplitApp.ViewModels;

namespace ExpenseSplitApp.Views
{
    public partial class ParticipantsPage : ContentPage
    {
        private ParticipantsViewModel _viewModel;

        public ParticipantsPage(int groupId)
        {
            InitializeComponent();
            _viewModel = new ParticipantsViewModel(groupId);
            BindingContext = _viewModel;
        }

        private async void OnAddParticipantClicked(object sender, EventArgs e)
        {
            var participantName = await DisplayPromptAsync("Nowy Uczestnik", "Podaj nazwê uczestnika:");
            await _viewModel.AddParticipantAsync(participantName);
        }

        private async void OnAddExpenseClicked(object sender, EventArgs e)
        {
            var description = await DisplayPromptAsync("Nowy Wydatek", "Podaj opis wydatku:");
            var amountString = await DisplayPromptAsync("Nowy Wydatek", "Podaj kwotê:");
            await _viewModel.AddExpenseAsync(description, amountString);
        }
    }
}
