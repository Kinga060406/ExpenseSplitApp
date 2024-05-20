using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseSplitApp.Models
{
    public static class DebtCalculator
    {
        public static async Task CalculateDebtsAsync(List<Participant> participants, List<Expense> expenses, Database database)
        {
            // Reset debts for all participants
            foreach (var participant in participants)
            {
                participant.Debt = 0m;
            }

            // Group expenses by GroupId
            var expensesByGroup = expenses.GroupBy(e => e.GroupId);

            foreach (var groupExpenses in expensesByGroup)
            {
                int groupId = groupExpenses.Key;
                var participantsInGroup = participants.Where(p => p.GroupId == groupId).ToList();

                if (participantsInGroup.Count > 0)
                {
                    decimal totalGroupExpense = groupExpenses.Sum(e => e.Amount);
                    decimal sharePerParticipant = totalGroupExpense / participantsInGroup.Count;

                    foreach (var participant in participantsInGroup)
                    {
                        participant.Debt = sharePerParticipant;
                        await database.UpdateParticipantAsync(participant);
                    }
                }
            }
        }
    }
}

