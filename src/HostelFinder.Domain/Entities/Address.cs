using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelFinder.Domain.Entities;

public class Address
{
    [Key]
    [ForeignKey("Hostel")]
    public Guid HostelId { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
    public string commune { get; set; }
    [MaxLength(255)]
    public string DetailAddress { get; set; }
    public virtual Hostel Hostel { get; set; }
}