namespace UserManagement.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Constants;
using UserManagement.Api.Models;
using UserManagement.Api.Services;


[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _service;
    public UsersController(UserService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get() => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var user = _service.GetById(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public IActionResult Post(User user)
    {
        try
        {
            _service.Create(user);
            return Ok(new { message = Success.CreateUser });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, User user)
    {
        try
        {
            user.UserId = id;
            _service.Update(user);
            return Ok(new { message = Success.UpdateUser });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _service.Delete(id);
            return Ok(new { message = Success.DeleteUser });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}