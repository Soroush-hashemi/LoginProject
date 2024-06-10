using LoginJwt.Model;
using LoginJwt.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoginJwt.Controllers;

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

    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
        await _userService.CreateAsync(newUser);

        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
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