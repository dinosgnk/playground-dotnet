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

    [HttpGet("GetUsers/{testValue}")]
    // public IActionResult Test()
    public string[] GetUsers(string testValue)
    {
        string[] responseArray = new string[] {
            "test1",
            "test2",
            testValue
        };
        return responseArray;
    }
}

