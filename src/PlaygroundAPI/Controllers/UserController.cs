using Microsoft.AspNetCore.Mvc;
using PlaygroundAPI.Data;

namespace PlaygroundAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT CURRENT_DATE");
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        string sql = @"
            SELECT
                UserId,
                FirstName,
                LastName,
                Email,
                Gender,
                Country,
                StreetAddress
            FROM
                UsersSchema.Users";

        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;
    }

    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        string sql = @"
            SELECT
                UserId,
                FirstName,
                LastName,
                Email,
                Gender,
                Country,
                StreetAddress
            FROM
                UsersSchema.Users
            WHERE
                UserId = " + userId.ToString();

        User user = _dapper.LoadDataSingle<User>(sql);
        Console.WriteLine(user);
        return user;
    }
}
