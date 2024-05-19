using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseSplitApp.Models;

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
        public Task<int> DeleteGroupAsync(Group group) => _database.DeleteAsync(group);

        
        public Task<List<Participant>> GetParticipantsAsync() => _database.Table<Participant>().ToListAsync();
        public Task<int> SaveParticipantAsync(Participant participant) => _database.InsertAsync(participant);
        public Task<int> DeleteParticipantAsync(Participant participant) => _database.DeleteAsync(participant);

       
        public Task<List<Expense>> GetExpensesAsync() => _database.Table<Expense>().ToListAsync();
        public Task<int> SaveExpenseAsync(Expense expense) => _database.InsertAsync(expense);
        public Task<int> DeleteExpenseAsync(Expense expense) => _database.DeleteAsync(expense);
    }
}
