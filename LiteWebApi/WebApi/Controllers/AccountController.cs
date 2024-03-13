using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Constants;
using WebApi.Services.Interfaces;
using WebApi.Services;
using Data.Entities.Identity;
using WebApi.Models.Account;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController(
	UserManager<User> userManager,
	IJwtTokenService jwtTokenService,
	IMapper mapper
	) : Controller {

	[HttpPost]
	public async Task<IActionResult> Login([FromForm] LoginViewModel model) {
		User? user = await userManager.Users
			.Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
			.FirstOrDefaultAsync(u => u.Email == model.Email);

		if (user is null || !await userManager.CheckPasswordAsync(user, model.Password))
			return BadRequest("Wrong authentication data");

		return Ok(new { Token = jwtTokenService.CreateToken(user) });
	}

	[HttpPost]
	public async Task<IActionResult> Registration([FromForm] RegisterViewModel model) {
		if (await userManager.FindByEmailAsync(model.Email) is not null)
			return BadRequest("The user with this email is already registered");

		User user = mapper.Map<RegisterViewModel, User>(model);

		try {
			user.Photo = await ImageWorker.SaveImageAsync(model.Image);
		}
		catch (Exception) {
			return BadRequest("Image error");
		}

		try {
			IdentityResult identityResult = await userManager.CreateAsync(user, model.Password);
			if (!identityResult.Succeeded)
				return BadRequest(identityResult.Errors.First().Description);

			identityResult = await userManager.AddToRoleAsync(user, Roles.User);

			if (!identityResult.Succeeded) {
				await userManager.DeleteAsync(user);
				ImageWorker.DeleteImageIfExists(user.Photo);
				return BadRequest("Role assignment error");
			}

			return Ok(new { Token = jwtTokenService.CreateToken(user) });
		}
		catch {
			ImageWorker.DeleteImageIfExists(user.Photo);
			throw;
		}
	}
}
