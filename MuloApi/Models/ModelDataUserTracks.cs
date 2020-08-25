using System.ComponentModel.DataAnnotations;

namespace MuloApi.Models
{
    public class ModelDataUserTracks : ModelUserTracks
    {
        [Required] public int Size { get; set; }
        [Required] public string TimeTrack { get; set; }
        [Required] public string StopPlayTime { get; set; }
    }
}