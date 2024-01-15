using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairShopApi.Data;
using RepairShopApi.Data.Database;

namespace RepairShopApi.Controllers;

public record CreateEditEmployeeModel(
    string Name,
    string Post);

[Route("api/[Controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly ILogger<EmployeesController> _logger;
    private readonly AppDbContext _dbContext;

    public EmployeesController(
        ILogger<EmployeesController> logger,
        AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public IEnumerable<Employee> GetEmployees()
    {
        return _dbContext.Employees
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .AsEnumerable();
    }
    
    [HttpGet("{id}")]
    public ActionResult<Employee> GetEmployee(
        [FromRoute] long id)
    {
        Employee? employee = _dbContext.Employees
            .Find(id);

        if (employee is null)
            return NotFound();

        return employee;
    }

    [HttpPost]
    public ActionResult<Employee> CreateEmployee(
        [FromBody] CreateEditEmployeeModel newEmployee)
    {
        Employee employee = EmployeeModelToEmployee(newEmployee);
        
        _dbContext.Employees.Add(employee);

        _dbContext.SaveChanges();

        return employee;
    }

    [HttpPut("{id}")]
    public ActionResult EditEmployee(
        [FromRoute] long id,
        [FromBody] CreateEditEmployeeModel editedEmployee)
    {
        if (!EmployeeExists(id))
            return NotFound();

        Employee employee = EmployeeModelToEmployee(editedEmployee);
        employee.Id = id;
        
        _dbContext.Entry(employee).State = EntityState.Modified;
        
        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")] 
    public ActionResult<Employee> DeleteEmployee(
        [FromRoute] long id)
    {
        Employee? employee = _dbContext.Employees
            .Find(id);
        
        if (employee is null)
            return NotFound();

        _dbContext.Employees.Remove(employee);

        _dbContext.SaveChanges();

        return employee;
    }

    private Employee EmployeeModelToEmployee(
        CreateEditEmployeeModel employeeModel)
    {
        return new Employee()
        {
            Name = employeeModel.Name,
            Post = employeeModel.Post
        };
    }

    private bool EmployeeExists(long id)
    {   
        return _dbContext.Employees.Any(x => x.Id == id);
    }
}
