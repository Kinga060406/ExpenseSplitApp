using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ExpenseSplitApp.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
