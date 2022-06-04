using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Business.Interfaces;
using VotingApp.Business.Models;
using VotingApp.Data.Data;

namespace VotingApp.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ViewParticipantInParty>> GetAllListOfParticipant()
        {
            var participants = await _dbContext.Participants
                .Include(x => x.Party)
                .Select(x => new ViewParticipantInParty
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Party = x.Party.Name
                })
                .ToListAsync();

            return participants;
        }

        public async Task<ViewParticipantInParty> GetParticipantById(int id)
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
                throw new Exception("Participant Not Found");
            }

            return participant;
        }

        public async Task Vote(int id, int userId)
        {
            var participant = await _dbContext.Participants
                .FirstOrDefaultAsync(x => x.Id == id);

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (participant == null)
            {
                throw new Exception("Participant Not Found");
            }
            if(user == null)
            {
                throw new Exception("User Not Found");
            }

            participant.VoteCount++;
            user.HasVoted = true;
            _dbContext.Participants.Update(participant);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
