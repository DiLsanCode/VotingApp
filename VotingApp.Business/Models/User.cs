using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Business.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string EGN { get; set; }
        public string IndividualAuthenticationCode { get; set; }
        public bool hasVoted { get; set; }

    }
}
