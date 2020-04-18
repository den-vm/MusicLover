using System;

namespace MuloApi.Models
{
    public class ModelUserTracks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DateLoad { get; internal set; }
    }
}