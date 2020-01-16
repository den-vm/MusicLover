using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuloApi.DataBase.Entities
{
    public class DBUser
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
