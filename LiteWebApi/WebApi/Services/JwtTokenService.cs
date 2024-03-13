using Data.Entities.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class JwtTokenService(IConfiguration _configuration) : IJwtTokenService {
	public string CreateToken(User user) {
		var key = Encoding.UTF8.GetBytes(
			_configuration["Jwt:SecretKey"]
				?? throw new NullReferenceException("Jwt:SecretKey")
		);

		int tokenLifetimeInDays = Convert.ToInt32(
			_configuration["Jwt:TokenLifetimeInDays"]
				?? throw new NullReferenceException("Jwt:TokenLifetimeInDays")
		);

		var signinKey = new SymmetricSecurityKey(key);

		var signinCredential = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

		var jwt = new JwtSecurityToken(
			signingCredentials: signinCredential,
			expires: DateTime.Now.AddDays(tokenLifetimeInDays),
			claims: GetClaims(user));

		return new JwtSecurityTokenHandler().WriteToken(jwt);
	}

	private static List<Claim> GetClaims(User user) {
		string userEmail = user.Email
			?? throw new NullReferenceException($"User.Email");

		if (user.UserRoles is null)
			throw new NullReferenceException("User.UserRoles");

		var roleClaims = user.UserRoles
			.Select(ur => ur.Role)
			.Select(r => new Claim(ClaimTypes.Role, r.Name!))
			.ToList();

		var claims = new List<Claim> {
			new ("email", userEmail),
			new ("name", $"{user.LastName} {user.FirstName}"),
			new ("photo", user.Photo)
		};
		claims.AddRange(roleClaims);

		return claims;
	}
}