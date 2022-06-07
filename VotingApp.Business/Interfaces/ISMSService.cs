using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Business.Interfaces
{
    public interface ISMSService
    {
        void SendMessage(string password, string phone);
    }
}
