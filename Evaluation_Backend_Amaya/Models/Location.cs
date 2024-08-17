using System.ComponentModel.DataAnnotations;

namespace Evaluation_Backend_Amaya.Models
{
    public class Location
    {
        [Required]
        public string code { get; set; }

        [Required]
        public string name { get; set; }
    }
}
