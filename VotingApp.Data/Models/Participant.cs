﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Data.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MIddleName { get; set; }
        public string LastName { get; set; }
        public int VoteCount { get; set; } = 0;
        public int PartyId { get; set; }
        public virtual Party Party { get; set; }

    }
}
