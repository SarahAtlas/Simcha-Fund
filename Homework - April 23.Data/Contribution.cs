using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework___April_23.Data
{
    public class Contribution
    {
        public int SimchaId { get; set; }
        public string SimchaName { get; set; }
        public DateTime SimchaDate { get; set; }
        public int ContributorId { get; set; }
        public decimal Amount { get; set; }
    }
}
