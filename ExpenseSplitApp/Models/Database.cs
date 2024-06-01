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
            _database.CreateTableAsync<User>().Wait();
        }

        // Użytkownicy
        public Task<List<User>> GetUsersAsync() => _database.Table<User>().ToListAsync();
        public Task<User> GetUserByUsernameAsync(string username) => _database.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
        public Task<User> GetUserByIdAsync(int userId) => _database.Table<User>().Where(u => u.Id == userId).FirstOrDefaultAsync();
        public Task<int> SaveUserAsync(User user) => _database.InsertAsync(user);
        public Task<int> UpdateUserAsync(User user) => _database.UpdateAsync(user);
        public Task<int> DeleteUserAsync(User user) => _database.DeleteAsync(user);

        // Grupy
        public Task<List<Group>> GetGroupsAsync(int userId) => _database.Table<Group>().Where(g => g.UserId == userId).ToListAsync();
        public Task<int> SaveGroupAsync(Group group) => _database.InsertAsync(group);
        public Task<int> UpdateGroupAsync(Group group) => _database.UpdateAsync(group);
        public Task<int> DeleteGroupAsync(Group group) => _database.DeleteAsync(group);

        // Uczestnicy
        public Task<List<Participant>> GetParticipantsAsync(int userId) => _database.Table<Participant>().Where(p => p.UserId == userId).ToListAsync();
        public Task<int> SaveParticipantAsync(Participant participant) => _database.InsertAsync(participant);
        public Task<int> UpdateParticipantAsync(Participant participant) => _database.UpdateAsync(participant);
        public Task<int> DeleteParticipantAsync(Participant participant) => _database.DeleteAsync(participant);

        public Task<int> DeleteParticipantsByGroupIdAsync(int groupId)
        {
            return _database.ExecuteAsync("DELETE FROM Participant WHERE GroupId = ?", groupId);
        }

        // Wydatki
        public Task<List<Expense>> GetExpensesAsync(int userId) => _database.Table<Expense>().Where(e => e.UserId == userId).ToListAsync();
        public Task<int> SaveExpenseAsync(Expense expense) => _database.InsertAsync(expense);
        public Task<int> UpdateExpenseAsync(Expense expense) => _database.UpdateAsync(expense);
        public Task<int> DeleteExpenseAsync(Expense expense) => _database.DeleteAsync(expense);

        public Task<int> DeleteExpensesByGroupIdAsync(int groupId)
        {
            return _database.ExecuteAsync("DELETE FROM Expense WHERE GroupId = ?", groupId);
        }
    }
}
