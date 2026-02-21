using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContactManagementSystem.Repositories
{
	public class DbInitializer : IHostedService
	{
		private readonly IServiceProvider _serviceProvider;

		public DbInitializer(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			using var scope = _serviceProvider.CreateScope();

			var userManager = scope.ServiceProvider
				.GetRequiredService<UserManager<IdentityUser>>();

			// Seed users
			await CreateUser(userManager, "user1@gmail.com");
			await CreateUser(userManager, "user2@gmail.com");
		}

		private async Task CreateUser(UserManager<IdentityUser> userManager,
									  string email)
		{
			var existingUser = await userManager.FindByEmailAsync(email);

			if (existingUser == null)
			{
				var user = new IdentityUser
				{
					UserName = email,
					Email = email,
					EmailConfirmed = true
				};

				// Password required internally — not used by your login
				await userManager.CreateAsync(user, "Temp@123456");
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
			=> Task.CompletedTask;
	}
}