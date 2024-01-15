using System.ComponentModel.DataAnnotations.Schema;

namespace RepairShopApi.Data;

#nullable disable

[Table("divice")]
public class Device
{
    [Column("id")]
    public long Id { get; set; }

    [Column("tipe")]
    public string Type { get; set; }

    [Column("model")]
    public string Model { get; set; }

    [Column("serial_number")]
    public string SerialNumber { get; set; }
}
