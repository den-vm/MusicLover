using System;
using System.ComponentModel.DataAnnotations;

namespace MuloApi.DataBase.Entities
{
    public class ModelCookieUser
    {
        public int Id { get; set; }

        [Required] public int IdUser { get; set; }

        [Required] public string Cookie { get; set; }
        [Required] public DateTime Start { get; set; }
        [Required] public DateTime End { get; set; }
    }
}