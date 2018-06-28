using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework___April_23.Data
{
    public class Deposit
    {
        public int Id { get; set; }
        public decimal DepositAmount { get; set; }
        public DateTime Date { get; set; }
        public int ContributorId { get; set; }
    }
}
