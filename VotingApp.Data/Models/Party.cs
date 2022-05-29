using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Data.Models
{
    public class Party
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Participant> Participant { get; set; }
    }
}
