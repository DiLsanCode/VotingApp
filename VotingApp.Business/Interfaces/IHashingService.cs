using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Business.Interfaces
{
    public interface IHashingService
    {
        string CustomHashing(string str);
    }
}
