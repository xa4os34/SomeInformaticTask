using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RepairShopApi.Data;

#nullable disable

[Table("request")]
public class Request
{
    [Column("id")]
    public long Id { get; set; }

    [Column("problem_description")]
    public string ProblemDescription { get; set; }

    [Column("priority")]
    public string Priority { get; set; }

    [Column("date_create")]
    public DateOnly CreatedAt { get; set; }

    [Column("status")]
    public string Status { get; set; }
    
    [ForeignKey("id_user")]
    public virtual User User { get; set; }

    [ForeignKey("id_device")]
    public virtual Device Device { get; set; }
}
