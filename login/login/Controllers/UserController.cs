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
        var book = await _userService.GetAsync(Id);

        if (book is null)
        {
            return NotFound();
        }

        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
        await _userService.CreateAsync(newUser);

        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    }
}