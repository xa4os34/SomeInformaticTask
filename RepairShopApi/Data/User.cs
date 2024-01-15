using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepairShopApi.Data;

#nullable disable

[Table("users")]
public class User
{
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("phone")]
    public string Phone { get; set; }

    [Column("date_registration")]
    public DateOnly RegistrationDate { get; set; }

    [Column("locations")]
    public string Location { get; set; }
}
