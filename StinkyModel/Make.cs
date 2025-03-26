using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StinkyModel;

[Table("make")]
public partial class Make
{
    [Key]
    [Column("makeID")]
    public int MakeId { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("Make")]
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
