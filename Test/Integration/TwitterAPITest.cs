using TwitterAPI.Infrastructure.Persistence;
using Autofac.Extras.Moq;
using TwitterAPI.Application.Domain;
using TwitterAPI.Presentation.Controllers;
using TwitterAPI.Application.Utils;

namespace Integration;

public class DatabaseFixture : IDisposable {
	public UserController UserController = null!;

	public DatabaseFixture() {
		using var mock = AutoMock.GetLoose();

		var sampleUsers = GetSampleUsers();
		var sampleUsersList = sampleUsers.ToList();

		// Arrange
		mock.Mock<ITwitterRepository>()
			.Setup(x => x.GetUsersAsync())
			.Returns(Task.FromResult(sampleUsers));

		mock.Mock<ITwitterRepository>()
			.Setup(x => x.GetUserAsync("vinciusb", false))
			.Returns(Task.FromResult(sampleUsersList[0]));

		mock.Mock<ITwitterRepository>()
			.Setup(x => x.GetUserAsync("Gringo", true))
			.Returns(Task.FromResult(sampleUsersList[3]));

		UserController = mock.Create<UserController>();
	}

	public void Dispose() {
	}

	public static IEnumerable<User> GetSampleUsers() {
		return new List<User>() {
			new() {
				Id = 1,
				Color = "FF0",
				At = "vinciusb",
				Username = "Vinícius Braga",
				Bio = "Só mais um dia na terra",
				City = "Belo Horizonte",
				Country = "Brazil",
				BirthDate = new DateOnly(2001, 6, 12),
			},
			new() {
				Id = 2,
				Color = "AF3",
				At = "junio.veras",
				Username = "Junio Verass",
				Bio = "IA",
				City = "Belo Horizonte",
				Country = "Brazil",
				BirthDate = null,
			},
			new() {
				Id = 3,
				Color = "0F0",
				At = "leoponcio",
				Username = "PoncioLEO",
				Bio = "...oi",
				City = "São Paulo",
				Country = "Brazil",
				BirthDate = new DateOnly(1967, 1, 12),
			},
			new()  {
				Id = 4,
				Color = "00A",
				At = "grings",
				Username = "Gringo",
				Bio = "I'm gringo",
				City = "Houston",
				Country = "USA",
				BirthDate = new DateOnly(1995, 8, 22),
			},
			new() {
				Id = 5,
				Color = "7FA",
				At = "laercy",
				Username = "Laerciu",
				Bio = "Sou maromba",
				City = "São Caetano do Sul",
				Country = "Brazil",
				BirthDate = new DateOnly(1985, 3, 29),
			},
		};
	}
}

public class TwitterAPITest : DatabaseFixture {
	[Fact]
	public async Task TestGetUsers_GetAllUsersRegistered() {
		// Arrange
		var expected = GetSampleUsers().ToList();

		// Act
		var actual = (await UserController.GetUsers(null, null, null)).ToList();

		// Assert
		Assert.True(actual != null);
		Assert.Equal(actual.Count, expected.Count);
		for(int i = 0; i < expected.Count; i++)
			Assert.Equal(expected[i].AsDTO(), actual[i]);
	}

	[Fact]
	public async Task TestGetUsers_GetRegisteredUserFromAt() {
		// Arrange
		var expected = GetSampleUsers().First();

		// Act
		var actual = (await UserController.GetUsers("vinciusb", null, null)).First();

		// Assert
		Assert.True(actual != null);
		Assert.Equal(expected.AsDTO(), actual);
	}

	[Fact]
	public async Task TestGetUsers_GetRegisteredUserFromUsername() {
		// Arrange
		var expected = GetSampleUsers().ToList()[3];

		// Act
		var actual = (await UserController.GetUsers(null, "Gringo", null)).First();

		// Assert
		Assert.True(actual != null);
		Assert.Equal(expected.AsDTO(), actual);
	}

	[Fact]
	public async Task TestGetUsers_GetUnexistingUser() {
		// Arrange

		// Act
		var actual = await UserController.GetUsers(null, "Mascarenhas", null);

		// Assert
		Assert.True(actual.Any());
	}
}