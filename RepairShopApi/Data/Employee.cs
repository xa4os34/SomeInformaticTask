using System.ComponentModel.DataAnnotations.Schema;

namespace RepairShopApi.Data;

#nullable disable

[Table("employees")]
public class Employee
{
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("post")]
    public string Post { get; set; }
}
