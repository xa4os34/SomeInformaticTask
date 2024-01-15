using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairShopApi.Data;
using RepairShopApi.Data.Database;

namespace RepairShopApi.Controllers;

public record CreateEditUserModel(
    [Required] string Name,
    [EmailAddress] string Email,
    [Phone] string Phone,
    string Location);

[Route("api/[Controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly AppDbContext _dbContext;

    public UsersController(
        ILogger<UsersController> logger,
        AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IEnumerable<User> GetUsers()
    {
        return _dbContext.Users
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .AsEnumerable();
    }

    [HttpGet("{id}")] 
    public ActionResult<User> GetUser(
        [FromQuery] long id)
    {
        User? user = _dbContext.Users
            .Find(id);

        if (user is null)
            return NotFound();

        return user;
    }

    [HttpGet("{id}/Requests")]
    public ActionResult<IEnumerable<Request>> GetUserRequests(
        [FromRoute] long id)
    {
        if (!UserExists(id))
            return NotFound();

        IEnumerable<Request> requests = _dbContext.Requests
            .Include(x => x.User)
            .Include(x => x.Device)
            .Where(x => x.User.Id == id);

        return new ActionResult<IEnumerable<Request>>(requests);
    }

    [HttpPost]
    public ActionResult<User> CreatUser(
        [FromBody] CreateEditUserModel newUser)
    {
        var user = UserModelToUser(newUser);

        _dbContext.Users.Add(user);
        
        _dbContext.SaveChanges();

        return user;
    }

    [HttpPut("{id}")]
    public ActionResult EditUser(
        [FromRoute] long id,
        [FromBody] CreateEditUserModel editedUser)
    {
        if (!UserExists(id))
            return NotFound();

        var user = UserModelToUser(editedUser);
        user.Id = id;

        _dbContext.Entry(user).State = EntityState.Modified;

        _dbContext.SaveChanges();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult<User> DeleteUser(
        [FromRoute] long id)
    {
        User? user = _dbContext.Users.Find(id);

        if (user is null) 
            return NotFound();
    _dbContext.Users.Remove(user);

        _dbContext.SaveChanges();

        return user;
    }

    private User UserModelToUser(
        CreateEditUserModel userModel)
    {
        return new User()
        {
            Name = userModel.Name,
            Email = userModel.Email,
            Phone = userModel.Phone,
            RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Location = userModel.Location
        };
    }

    private bool UserExists(long id)
    {
        return _dbContext.Users
            .Any(x => x.Id == id);
    }
}
