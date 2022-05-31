using Microsoft.AspNetCore.Mvc;
using VotingApp.Business.Interfaces;
using VotingApp.Business.Models;
using VotingApp.Data.Data;
using VotingApp.Data.Models;

namespace VotingApp.Business.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Participant> AddParticipant(RegisterNewParticipant newParticipant)
        {
            var result = await _dbContext.AddAsync(new Participant()
            {
                FirstName = UppercaseFirst(newParticipant.FirstName),
                MIddleName = UppercaseFirst(newParticipant.MiddleName),
                LastName = UppercaseFirst(newParticipant.LastName),
                PartyId = newParticipant.PartyId,
            });
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public Task<IActionResult> DeleteParticipant()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> EditParticipant()
        {
            throw new NotImplementedException();
        }

        private string UppercaseFirst(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
    }
}
