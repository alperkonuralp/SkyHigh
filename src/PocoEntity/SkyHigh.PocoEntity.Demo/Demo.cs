using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SkyHigh.PocoEntity.Demo;

public class Demo
{
    [Required]
    [MaxLength(100)]
    [DefaultValue(100)]
    public int Id { get; set; }
}