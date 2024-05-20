using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace ExpenseSplitApp.Models
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Group>().Wait();
            _database.CreateTableAsync<Participant>().Wait();
            _database.CreateTableAsync<Expense>().Wait();
        }

        public Task<List<Group>> GetGroupsAsync() => _database.Table<Group>().ToListAsync();
        public Task<int> SaveGroupAsync(Group group) => _database.InsertAsync(group);
        public Task<int> UpdateGroupAsync(Group group) => _database.UpdateAsync(group);
        public Task<int> DeleteGroupAsync(Group group) => _database.DeleteAsync(group);


        public Task<List<Participant>> GetParticipantsAsync() => _database.Table<Participant>().ToListAsync();
        public Task<int> SaveParticipantAsync(Participant participant) => _database.InsertAsync(participant);
        public Task<int> UpdateParticipantAsync(Participant participant) => _database.UpdateAsync(participant);
        public Task<int> DeleteParticipantAsync(Participant participant)
        {
            return _database.DeleteAsync(participant);
        }


        public Task<List<Expense>> GetExpensesAsync() => _database.Table<Expense>().ToListAsync();
        public Task<int> SaveExpenseAsync(Expense expense)
        {
            return _database.InsertOrReplaceAsync(expense);
        }
        public Task<int> DeleteExpenseAsync(Expense expense)
        {
            return _database.DeleteAsync(expense);
        }

        // New methods to delete participants and expenses by group id
        public Task<int> DeleteParticipantsByGroupIdAsync(int groupId)
        {
            return _database.ExecuteAsync("DELETE FROM Participant WHERE GroupId = ?", groupId);
        }

        public Task<int> DeleteExpensesByGroupIdAsync(int groupId)
        {
            return _database.ExecuteAsync("DELETE FROM Expense WHERE GroupId = ?", groupId);
        }
    }
}
