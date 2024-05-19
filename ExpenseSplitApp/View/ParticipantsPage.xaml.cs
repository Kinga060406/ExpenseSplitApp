using ExpenseSplitApp.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseSplitApp.View
{
    public partial class ParticipantsPage : ContentPage
    {
        private ObservableCollection<Participant> _participants;
        private int _groupId;

        public ParticipantsPage(int groupId)
        {
            InitializeComponent();
            _groupId = groupId;
            LoadParticipants();
        }

        private async void LoadParticipants()
        {
            var participants = await App.Database.GetParticipantsAsync();
            _participants = new ObservableCollection<Participant>(participants.Where(p => p.GroupId == _groupId));
            participantsListView.ItemsSource = _participants;
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
    }
}
