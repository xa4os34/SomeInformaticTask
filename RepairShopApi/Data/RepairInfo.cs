using System.ComponentModel.DataAnnotations.Schema;

namespace RepairShopApi.Data;

#nullable disable

[Table("repair_record")]
public class RepairInfo
{
    [Column("id")]
    public long Id { get; set; }

    [Column("date_compection")]
    public DateOnly CompletionDate { get; set; }

    [ForeignKey("id_request")]
    public Request Request { get; set; }

    [ForeignKey("id_device")]
    public Device Device { get; set; }

    [ForeignKey("id_employees")]
    public Employee Employee { get; set; }
}
