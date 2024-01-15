using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairShopApi.Data;
using RepairShopApi.Data.Database;

namespace RepairShopApi.Controllers;

public record CreateRequestModel(
    string ProblemDescription,
    string Priority,
    string Status,
    long UserId,
    CreateEditDeviceModel Device);

public record EditRequestModel(
    string ProblemDescription,
    string Priority,
    string Status,
    long UserId);

public record CreateEditDeviceModel(
    string Type,
    string Model,
    string SerialNumber);

public record CreateEditRepairInfoModel(
    DateOnly CompletionDate,
    long EmployeeId);

public record RepairInfoDto(
    long Id,
    DateOnly CompletionDate,
    Device Device,
    Employee Employee);


[Route("api/[Controller]")]
[ApiController]
public class RequestsController : ControllerBase
{
    private readonly ILogger<RequestsController> _logger;
    private readonly AppDbContext _dbContext;

    public RequestsController(
        ILogger<RequestsController> logger,
        AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IEnumerable<Request> GetRequests()
    {
        return _dbContext.Requests
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Include(x => x.Device)
            .Include(x => x.User)
            .AsEnumerable();
    }

    [HttpGet("{id}")]
    public ActionResult<Request> GetRequest(
        [FromRoute] long id)
    {
        Request? request = _dbContext.Requests
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Device)
            .FirstOrDefault(x => x.Id == id);
        
        if (request is null)
            return NotFound();

        return request;
    }

    [HttpGet("{id}/RepairInfo")]
    public ActionResult<RepairInfoDto> GetRepairInfo(
        [FromRoute] long id)
    {
        RepairInfo? repairInfo = _dbContext.Repairs
            .AsNoTracking()
            .Include(x => x.Device)
            .Include(x => x.Employee)
            .FirstOrDefault(x => x.Request.Id == id);

        if (repairInfo is null)
            return NotFound();

        return new RepairInfoDto(
            Id: repairInfo.Id,
            CompletionDate: repairInfo.CompletionDate,
            Device: repairInfo.Device,
            Employee: repairInfo.Employee);
    }

    [HttpGet("{id}/Device")]
    public ActionResult<Device> GetDevice(
        [FromRoute] long id)
    {
        Request? request = _dbContext.Requests
            .AsNoTracking()
            .Include(x => x.Device)
            .FirstOrDefault(x => x.Id == id);

        if (request is null)
            return NotFound();

        return request.Device;
    }

    [HttpPost]
    public ActionResult<Request> CreateRequest(
        [FromBody] CreateRequestModel newRequest)
    {
        User? user = _dbContext.Users
            .Find(newRequest.UserId);

        if (user is null)
        {
            ModelState.AddModelError(
                nameof(CreateRequestModel.UserId),
                "User with this id does not exist");

            return ValidationProblem();
        }

        Request request = RequestModelToRequest(newRequest);
        request.User = user;

        _dbContext.Requests.Add(request)
            .Reference(x => x.User);

        _dbContext.SaveChanges();

        return request;
    }

    [HttpPut("{id}")]
    public ActionResult EditRequest(
        [FromRoute] long id,
        [FromBody] EditRequestModel editedRequest)
    {
        if (!RequestExists(id))
            return NotFound();

        User? user = _dbContext.Users
            .Find(editedRequest.UserId);

        if (user is null)
        {
            ModelState.AddModelError(
                nameof(CreateRequestModel.UserId),
                "User with this id does not exist");

            return ValidationProblem();
        }

        Request request = RequestModelToRequest(editedRequest);
        request.Id = id;
        
        _dbContext.Entry(request).State = EntityState.Modified;

        _dbContext.SaveChanges();
        
        return NoContent();
    }

    [HttpPost("{id}/RepairInfo")]
    public ActionResult CreateRepairInfo(
        long id,
        CreateEditRepairInfoModel newRepairInfo)
    {
        Request? request = _dbContext.Requests
            .AsNoTracking()
            .Include(x => x.Device)
            .FirstOrDefault(x => x.Id == id);
    
        if (request is null)
            return NotFound();

        if (_dbContext.Repairs.Any(x => x.Request == request))
            return BadRequest("Repair info already exist.");

        Employee? employee = _dbContext.Employees
            .Find(newRepairInfo.EmployeeId);

        if (employee is null)
        {
            ModelState.AddModelError(
                nameof(CreateEditRepairInfoModel.EmployeeId),
                "Employee with this id does not exist.");

            return ValidationProblem();
        }

        RepairInfo repairInfo = new RepairInfo()
        {
            CompletionDate = newRepairInfo.CompletionDate
        };

        _dbContext.Repairs.Add(repairInfo);

        repairInfo.Request = request;
        repairInfo.Device = request.Device;
        repairInfo.Employee = employee;

        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpPut("{id}/RepairInfo")]
    public ActionResult EditRepairInfo(
        long id,
        CreateEditRepairInfoModel editedRepairInfo)
    {
        RepairInfo? repairInfo = _dbContext.Repairs
            .FirstOrDefault(x => x.Request.Id == id);
    
        if (repairInfo is null)
            return NotFound();

        Employee? employee = _dbContext.Employees
            .Find(editedRepairInfo.EmployeeId);

        if (employee is null)
        {
            ModelState.AddModelError(
                nameof(CreateEditRepairInfoModel.EmployeeId),
                "Employee with this id does not exist.");

            return ValidationProblem();
        }

        repairInfo.Employee = employee;
        repairInfo.CompletionDate = editedRepairInfo.CompletionDate;

        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpPut("{id}/Device")]
    public ActionResult EditDevice(
        [FromRoute] long id,
        [FromBody] CreateEditDeviceModel editedDevice)
    {
        Request? request = _dbContext.Requests
            .AsNoTracking()
            .Include(x => x.Device)
            .FirstOrDefault(x => x.Id == id);

        if (request is null)
            return NotFound();

        Device device = DeviceModelToDevice(editedDevice);
        device.Id = request.Device.Id;

        _dbContext.Entry(device).State = EntityState.Modified;

        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult<Request> DeleteRequest(
        [FromRoute] long id)
    {
        Request? request = _dbContext.Requests
            .Include(x => x.Device)
            .FirstOrDefault(x => x.Id == id);

        if (request is null) 
            return NotFound();

        IEnumerable<RepairInfo> repairs = _dbContext.Repairs
            .Where(x => x.Request.Id == id)
            .AsEnumerable();

        _dbContext.Repairs.RemoveRange(repairs);
        _dbContext.Devices.Remove(request.Device);
        _dbContext.Requests.Remove(request);

        _dbContext.SaveChanges();

        return request;
    }

    [HttpDelete("{id}/RepairInfo")]
    public ActionResult<RepairInfo> DeleteRepairRecord(
        [FromRoute] long id)
    {
        RepairInfo? repairInfo = _dbContext.Repairs
            .FirstOrDefault(x =>x.Request.Id == id);

        if (repairInfo is null)
            return NotFound();

        _dbContext.Repairs.Remove(repairInfo);

        _dbContext.SaveChanges();

        return repairInfo;
    }

    private Request RequestModelToRequest(
        CreateRequestModel requestModel)
    {
        return new Request()
        {
            ProblemDescription = requestModel.ProblemDescription,
            Priority = requestModel.Priority,
            Status = requestModel.Status,
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
            Device = DeviceModelToDevice(requestModel.Device),
        };
    }

    private Request RequestModelToRequest(
        EditRequestModel requestModel)
    {
        return new Request()
        {
            ProblemDescription = requestModel.ProblemDescription,
            Priority = requestModel.Priority,
            Status = requestModel.Status,
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
        };
    }

    private Device DeviceModelToDevice(
        CreateEditDeviceModel deviceModel)
    {
        return new Device()
        {   
            Type = deviceModel.Type,
            Model = deviceModel.Model,
            SerialNumber = deviceModel.SerialNumber
        };
    }

    private bool RequestExists(long id)
    {
        return _dbContext.Requests
            .Any(x => x.Id == id);
    }
}
