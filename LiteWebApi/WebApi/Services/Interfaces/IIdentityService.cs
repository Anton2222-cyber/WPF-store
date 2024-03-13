using Data.Entities.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Services.Interfaces;

public interface IIdentityService {
	Task<User> GetCurrentUserAsync(ControllerBase controller);
}