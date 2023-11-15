using TwitterAPI.Application.Domain;
using TwitterAPI.Infrastructure.Persistence;

namespace Unit;

public class TwitterRepositoryTest : PostgresTwitterRepository {
	public TwitterRepositoryTest() : base("") { }

	[Fact]
	public void TestGetTweetTreeDescriptionRecursively_NoLikedTweetsInDeepTree() {
		// Arrange
		Tweet root = new() {
			Id = 1, Likes = 0,
			Replies = new List<Tweet>() {
				new() {
					Id = 2, Likes = 0,
					Replies = new List<Tweet>() {
						new() {
							Id = 3, Likes = 0,
							Replies = new List<Tweet>() {
								new() {
									Id = 4, Likes = 0,
									Replies = new List<Tweet>() {
										new() {
											Id = 5, Likes = 0,
											Replies = new List<Tweet>() {
												new() {
													Id = 6, Likes = 0,
													Replies = new List<Tweet>() {

													},
												},
											},
										},
									},
								},
							},
						},
					},
				},
			},
		};
		IDictionary<int, (int, int)> expectedLikesAndRepliesDict = new Dictionary<int, (int, int)>() {
			{1, (0, 5)},
			{2, (0, 4)},
			{3, (0, 3)},
			{4, (0, 2)},
			{5, (0, 1)},
			{6, (0, 0)},
		};
		var expectedTotalChildrenNumber = 5;

		// Act
		var actualLikesAndRepliesDict = new Dictionary<int, (int, int)>();
		var actualTotalChildrenNumber = GetTreeDescriptionRecursively(root, actualLikesAndRepliesDict);

		// Assert
		Assert.Equal(expectedTotalChildrenNumber, actualTotalChildrenNumber);
		Assert.Equal(actualLikesAndRepliesDict.Count, expectedLikesAndRepliesDict.Count);
		Assert.All(actualLikesAndRepliesDict,
				   pair => Assert.True(expectedLikesAndRepliesDict.Contains(pair)));
	}

	[Fact]
	public void TestGetTweetTreeDescriptionRecursively_SomeLikedTweetsInDeepTree() {
		// Arrange
		Tweet root = new() {
			Id = 1, Likes = 3,
			Replies = new List<Tweet>() {
				new() {
					Id = 2, Likes = 0,
					Replies = new List<Tweet>() {
						new() {
							Id = 3, Likes = 0,
							Replies = new List<Tweet>() {
								new() {
									Id = 4, Likes = 6,
									Replies = new List<Tweet>() {
										new() {
											Id = 5, Likes = 0,
											Replies = new List<Tweet>() {
												new() {
													Id = 6, Likes = 8,
													Replies = new List<Tweet>() {

													},
												},
											},
										},
									},
								},
							},
						},
					},
				},
			},
		};
		IDictionary<int, (int, int)> expectedLikesAndRepliesDict = new Dictionary<int, (int, int)>() {
			{1, (3, 5)},
			{2, (0, 4)},
			{3, (0, 3)},
			{4, (6, 2)},
			{5, (0, 1)},
			{6, (8, 0)},
		};
		var expectedTotalChildrenNumber = 5;

		// Act
		var actualLikesAndRepliesDict = new Dictionary<int, (int, int)>();
		var actualTotalChildrenNumber = GetTreeDescriptionRecursively(root, actualLikesAndRepliesDict);

		// Assert
		Assert.Equal(expectedTotalChildrenNumber, actualTotalChildrenNumber);
		Assert.Equal(actualLikesAndRepliesDict.Count, expectedLikesAndRepliesDict.Count);
		Assert.All(actualLikesAndRepliesDict,
				   pair => Assert.True(expectedLikesAndRepliesDict.Contains(pair)));
	}

	[Fact]
	public void TestGetTweetTreeDescriptionRecursively_NoRepliesOnTweet() {
		// Arrange
		Tweet root = new() {
			Id = 1, Likes = 3,
			Replies = new List<Tweet>(),
		};
		IDictionary<int, (int, int)> expectedLikesAndRepliesDict = new Dictionary<int, (int, int)>() {
			{1, (3, 0)},
		};
		var expectedTotalChildrenNumber = 0;

		// Act
		var actualLikesAndRepliesDict = new Dictionary<int, (int, int)>();
		var actualTotalChildrenNumber = GetTreeDescriptionRecursively(root, actualLikesAndRepliesDict);

		// Assert
		Assert.Equal(expectedTotalChildrenNumber, actualTotalChildrenNumber);
		Assert.Equal(actualLikesAndRepliesDict.Count, expectedLikesAndRepliesDict.Count);
		Assert.All(actualLikesAndRepliesDict,
				   pair => Assert.True(expectedLikesAndRepliesDict.Contains(pair)));
	}
	[Fact]
	public void Test4() {
		// Arrange
		// Act
		// Assert
	}
	[Fact]
	public void Test5() {
		// Arrange
		// Act
		// Assert
	}
	[Fact]
	public void Test6() {
		// Arrange
		// Act
		// Assert
	}
	[Fact]
	public void Test7() {
		// Arrange
		// Act
		// Assert
	}
	[Fact]
	public void Test8() {
		// Arrange
		// Act
		// Assert
	}
	[Fact]
	public void Test9() {
		// Arrange
		// Act
		// Assert
	}
	[Fact]
	public void Test10() {
		// Arrange
		// Act
		// Assert
	}
	[Fact]
	public void Test11() {
		// Arrange
		// Act
		// Assert
	}
	[Fact]
	public void Test12() {
		// Arrange
		// Act
		// Assert
	}
	[Fact]
	public void Test13() {
		// Arrange
		// Act
		// Assert
	}
	[Fact]
	public void Test14() {
		// Arrange
		// Act
		// Assert
	}
	[Fact]
	public void Test15() {
		// Arrange
		// Act
		// Assert
	}
}

