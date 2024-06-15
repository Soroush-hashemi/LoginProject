using login.Model;
using login.Services;
using Microsoft.AspNetCore.Mvc;

namespace login.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService) =>
        _userService = userService;

    [HttpGet]
    public async Task<List<User>> Get() =>
        await _userService.GetListAsync();

    [HttpGet("{Id}")]
    public async Task<ActionResult<User>> Get(long Id)
    {
        var User = await _userService.GetAsync(Id);

        if (User is null)
        {
            return NotFound();
        }

        return User;
    }

    [HttpPost("{SignUp}")]
    public async Task<IActionResult> SignUp(User newUser)
    {
        if (newUser == null)
            throw new Exception();

        await _userService.CreateAsync(newUser);

        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    }

    [HttpPost("{SignIn}")]
    public string SignIn(string email, string pasword)
    {
        if (email is null)
            throw new Exception();
        if (pasword is null)
            throw new Exception();

        var token = _userService.GenerateToken(email, pasword);

        return token;
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> Edit(long Id, User UpdatedUser)
    {
        var user = await _userService.GetAsync(Id);

        if (user is null)
        {
            return NotFound();
        }

        UpdatedUser.Id = user.Id;
        await _userService.UpdateAsync(Id, UpdatedUser);

        return NoContent();
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(long Id)
    {
        var user = _userService.GetAsync(Id);

        if (user is null)
        {
            return NotFound();
        }

        await _userService.RemoveAsync(Id);
        return NoContent();
    }
}