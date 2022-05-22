using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Business.Constants;

namespace VotingApp.Business.Models
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.Password)]
        public string EGN { get; set; }
        public string PhoneNumber { get; set; }
        //public Roles Role { get; set; }
    }
}
