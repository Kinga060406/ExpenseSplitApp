using System.Collections.Generic;
using System.Linq;
using ExpenseSplitApp.Models;

namespace ExpenseSplitApp.Services
{
    public class DebtCalculator
    {
        public static void CalculateDebts(List<Participant> participants, List<Expense> expenses)
        {
            foreach (var participant in participants)
            {
                // Wyzeruj zadłużenie dla każdego uczestnika
                participant.Debt = 0m;
            }

            // Obliczanie sumy wszystkich wydatków w grupie
            decimal totalExpenseAmount = expenses.Sum(expense => expense.Amount);

            // Obliczanie zadłużenia dla każdego uczestnika
            foreach (var expense in expenses)
            {
                // Obliczanie równego podziału wydatku pomiędzy wszystkich uczestników
                decimal sharePerParticipant = expense.Amount / participants.Count;

                // Dodawanie udziału wydatku do zadłużenia każdego uczestnika
                foreach (var participant in participants)
                {
                    if (participant.GroupId == expense.GroupId)
                    {
                        participant.Debt += sharePerParticipant;
                    }
                }
            }
        }
    }
}

