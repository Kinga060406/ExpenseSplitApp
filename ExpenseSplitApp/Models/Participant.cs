using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSplitApp.Models
{
    public class Participant
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int GroupId { get; set; }
        public decimal Debt { get; set; }
        public int UserId { get; set; }
    }
}
