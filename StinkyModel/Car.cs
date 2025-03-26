using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StinkyModel;

[Table("car")]
public partial class Car
{
    [Key]
    [Column("carID")]
    public int CarId { get; set; }

    [Column("model")]
    [StringLength(50)]
    public string Model { get; set; } = null!;

    [Column("year")]
    public int Year { get; set; }

    [Column("makeID")]
    public int MakeId { get; set; }

    [ForeignKey("MakeId")]
    [InverseProperty("Cars")]
    public virtual Make Make { get; set; } = null!;
}
