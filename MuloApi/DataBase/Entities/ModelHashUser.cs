using System.ComponentModel.DataAnnotations;

namespace MuloApi.DataBase.Entities
{
    public class ModelHashUser
    {
        public int Id { get; set; }

        [Required] public int IdUser { get; set; }

        [Required] public string HashUser { get; set; }
    }
}