using System.ComponentModel.DataAnnotations;

namespace VotingApp.Business.Models
{
    public class RegisterNewParticipant
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int PartyId { get; set; }

    }
}
