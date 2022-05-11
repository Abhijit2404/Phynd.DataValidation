using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Phynd.DataValidation.Models
{
    public class DataAnalysis_User
    {
        [Key]
        [Column("Id", TypeName = "int(11)")]
        public int Id { get; set; }
        [Required]
        [Column("UserName")]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [Column("Password")]
        [StringLength(50)]
        public string Password { get; set; }
    }
}
