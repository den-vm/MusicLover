using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MuloApi.DataBase.Entities
{
    public class ExistUser
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
    }
}
