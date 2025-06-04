using EduVault.Models;
using EduVault.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EduVault.Controllers
{
	[ApiController]
	[Route("api/users")]
	public class UserController : ControllerBase
	{
		private readonly IUserRepository _repository;

		public UserController(IUserRepository repository)
		{
			_repository = repository;
		}

		[HttpPost]
		public async Task<IActionResult> CreateUser(User user)
		{
			await _repository.AddAsync(user);
			return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetUser(int id)
		{
			var user = await _repository.GetByIdAsync(id);
			return user != null ? Ok(user) : NotFound();
		}
	}
}
