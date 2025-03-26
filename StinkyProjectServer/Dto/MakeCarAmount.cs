using StinkyModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StinkyProjectServer.Dto
{
    public class MakeCarAmount
    {

        public int MakeId { get; set; }

        public string Name { get; set; } = null!;

        public int CarAmount { get; set; }
    }
}
