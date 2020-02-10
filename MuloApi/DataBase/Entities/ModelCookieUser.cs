using System.ComponentModel.DataAnnotations;

namespace MuloApi.DataBase.Entities
{
    public class ModelCookieUser
    {
        public int Id { get; set; }

        [Required] public int IdUser { get; set; }

        [Required] public string Cookie { get; set; }
    }
}