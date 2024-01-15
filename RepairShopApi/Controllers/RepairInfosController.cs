using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairShopApi.Data;
using RepairShopApi.Data.Database;

namespace RepairShopApi.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class RepairInfosController : ControllerBase
{
    private readonly ILogger<RepairInfosController> _logger;
    private readonly AppDbContext _dbContext;

    public RepairInfosController(
        ILogger<RepairInfosController> logger,
        AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IEnumerable<RepairInfo> GetRepairs()
    {
        return _dbContext.Repairs
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Include(x => x.Request)
            .Include(x => x.Request.Device)
            .Include(x => x.Request.User)
            .Include(x => x.Device)
            .Include(x => x.Employee)
            .AsEnumerable();
    }
}
