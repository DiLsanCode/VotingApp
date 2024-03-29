﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VotingApp.Business.Interfaces;
using VotingApp.Business.Models;
using VotingApp.Data.Data;
using VotingApp.Data.Models;

namespace VotingApp.Business.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly INamingService _naming;

        public AdminService(ApplicationDbContext dbContext, 
                            INamingService naming)
        {
            _dbContext = dbContext;
            _naming = naming;
        }

        public async Task<Participant> AddParticipant(RegisterNewParticipant newParticipant)
        {
            var result = await _dbContext.AddAsync(new Participant()
            {
                FirstName = _naming.UppercaseFirst(newParticipant.FirstName),
                MIddleName = _naming.UppercaseFirst(newParticipant.MiddleName),
                LastName = _naming.UppercaseFirst(newParticipant.LastName),
                PartyId = newParticipant.PartyId,
            });
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Party> AddParty(RegisterNewParty newParty)
        {
            var result = await _dbContext.AddAsync(new Party()
            {
                Name = _naming.UppercaseFirst(newParty.Name),
            });
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<ViewParticipantInParty> GetParticipant(int id)
        {
            var participant = await _dbContext.Participants
                .Select(x => new ViewParticipantInParty
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Party = x.Party.Name
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            if(participant == null)
            {
                throw new Exception("User Not Found!");
            }
            return participant;
        }

        public async Task<RegisterNewParticipant> GetParticipantForEdit(int id)
        {
            var participant = await _dbContext.Participants
                .Select(x => new RegisterNewParticipant
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    MiddleName = x.MIddleName,
                    LastName = x.LastName,
                    PartyId = x.PartyId,
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            if(participant == null)
            {
                throw new Exception("User Not Found!");
            }
            return participant;
        }

        public async Task DeleteParticipant(int id)
        {
            var participantToRemove = await _dbContext.Participants
               .FirstOrDefaultAsync(x => x.Id == id);

            if(participantToRemove == null)
            {
                throw new Exception("'Participant Not Found");
            }

            _dbContext.Participants.Remove(participantToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditParticipant(int id, RegisterNewParticipant participant)
        {
            var participantToEdit = await _dbContext.Participants
               .FirstOrDefaultAsync(x => x.Id == id);

            if (participantToEdit != null)
            {
                participantToEdit.FirstName = participant.FirstName;
                participantToEdit.MIddleName = participant.MiddleName;
                participantToEdit.LastName = participant.LastName;
                participantToEdit.PartyId = participant.PartyId;

                var updatedOrder = _dbContext.Participants.Update(participantToEdit);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Participant Not Found");
            }
        }

        public async Task<IEnumerable<ViewParty>> GetListOfParties()
        {
            var parties = await _dbContext.Parties
                .Select(x => new ViewParty
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToListAsync();

            return parties;
        }
    }
}
