using Microsoft.EntityFrameworkCore;
using Phynd.DataValidation.Data;
using Phynd.DataValidation.Interface;
using Phynd.DataValidation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phynd.DataValidation.Services
{
    public class Auth : IAuth
    {
        private readonly phyndContext _context;
        public Auth(phyndContext context)
        {
            _context = context;
        }
        public async Task<DataAnalysis_User> LoginUser(string username, string password)
        {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return null;
                }
                return await _context.DataAnalysis_User.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }
    }
}
