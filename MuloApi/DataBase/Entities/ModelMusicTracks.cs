using System;
using System.ComponentModel.DataAnnotations;

namespace MuloApi.DataBase.Entities
{
    public class ModelMusicTracks
    {
        public int Id { get; set; }
        [Required] public int IdCatalog { get; set; }
        [Required] public int IdUser { get; set; }
        [Required] public int IdTrack { get; set; }

        [Required] public string NameTrack { get; set; }

        [Required] public DateTime DateLoadTrack { get; set; }
    }
}