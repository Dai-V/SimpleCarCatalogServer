using StinkyModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StinkyProjectServer.Dto
{
    public class CarMake
    {
        public int CarId { get; set; }

        public string Model { get; set; } = null!;

        public int Year { get; set; }

        public string Make { get; set; } = null!;

    }
}
