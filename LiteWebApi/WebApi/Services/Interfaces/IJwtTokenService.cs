using Data.Entities.Identity;

namespace WebApi.Services.Interfaces;

public interface IJwtTokenService {
	string CreateToken(User user);
}