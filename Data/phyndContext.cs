using Microsoft.EntityFrameworkCore;
using Phynd.DataValidation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phynd.DataValidation.Data
{
    public class phyndContext : DbContext
    {
        public phyndContext(DbContextOptions<phyndContext> options) : base(options)
        {

        }

        public DbSet<DataAnalysis_User> DataAnalysis_User { get; set; }
    }
}
