﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Business.Models;
using VotingApp.Data.Models;

namespace VotingApp.Business.Interfaces
{
    public interface IAdminService
    {
        Task<Participant> AddParticipant(RegisterNewParticipant newParticipant);
        Task<Party> AddParty(RegisterNewParty newParty);
        Task DeleteParticipant(int id);
        Task<IActionResult> EditParticipant();
    }
}
