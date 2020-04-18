using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MuloApi.DataBase.Entities
{
    public class ModelCatalog
    {
        public int Id { get; set; }

        [Required] public int IdUser { get; set; }

        [Required] public string Path { get; set; }
    }
}
