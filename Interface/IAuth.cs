using Phynd.DataValidation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phynd.DataValidation.Interface
{
    public interface IAuth
    {
        Task<DataAnalysis_User> LoginUser(string username, string password);
    }
}
