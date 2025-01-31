using E_Commerce.API.Controllers;
using E_Commerce.Data.Models.Security;
using E_Commerce.Services.DTOs;
using E_Commerce.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
public class AccountsController : BaseController
{
    private readonly UserManager<UserSet> _userManager;
    private readonly RoleManager<RoleSet> _roleManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AccountsController(UserManager<UserSet> userManager,
        RoleManager<RoleSet> roleManager,
        IJwtTokenService jwtTokenService
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = new UserSet { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            if (!await _roleManager.RoleExistsAsync(model.Role))
                await _roleManager.CreateAsync(new RoleSet { Name = model.Role });

            await _userManager.AddToRoleAsync(user, model.Role);
            return Ok("Registration successful");
        }
        return BadRequest(result.Errors);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized();
        }

        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        
        var tokenString = _jwtTokenService.GenerateToken(claims, user.Email!, user.UserName!);
        return Ok(new { Token = tokenString });
    }
}
