using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSplitApp.Models
{
    public class Expense
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
}
