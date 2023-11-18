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
	public void TestGetTweetTreeDescriptionRecursively_NoRepliesOnLikedTweet() {
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
	public void TestGetTweetTreeDescriptionRecursively_NoRepliesOnNotLikedTweet() {
		// Arrange
		Tweet root = new() {
			Id = 1, Likes = 0,
			Replies = new List<Tweet>(),
		};
		IDictionary<int, (int, int)> expectedLikesAndRepliesDict = new Dictionary<int, (int, int)>() {
			{1, (0, 0)},
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
	public void TestGetTweetTreeDescriptionRecursively_SomeLikedTweetsInWideAndDeepTree() {
		// Arrange
		Tweet root = new() {
			Id = 1, Likes = 3,
			Replies = new List<Tweet>() {
				new() {
					Id = 2, Likes = 0,
					Replies = new List<Tweet>() {
						new() {
							Id = 3, Likes = 0,
							Replies = new List<Tweet>() {}
						},
						new() {
							Id = 10, Likes = 2,
							Replies = new List<Tweet>() {}
						},
					},
				},
				new() {
					Id = 4, Likes = 2,
					Replies = new List<Tweet>() {}
				},
				new() {
					Id = 5, Likes = 2,
					Replies = new List<Tweet>() {
						new() {
							Id = 7, Likes = 2,
							Replies = new List<Tweet>() {
								new() {
									Id = 8, Likes = 2,
									Replies = new List<Tweet>() {}
								},
								new() {
									Id = 9, Likes = 2,
									Replies = new List<Tweet>() {}
								},
							}
						},
					}
				},
				new() {
					Id = 6, Likes = 2,
					Replies = new List<Tweet>() {}
				},
			},
		};
		IDictionary<int, (int, int)> expectedLikesAndRepliesDict = new Dictionary<int, (int, int)>() {
			{1, (3, 9)},
			{2, (0, 2)},
			{3, (0, 0)},
			{4, (2, 0)},
			{5, (2, 3)},
			{6, (2, 0)},
			{7, (2, 2)},
			{8, (2, 0)},
			{9, (2, 0)},
			{10, (2, 0)},
		};
		var expectedTotalChildrenNumber = 9;

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
	public void TestGetTweetTreeDescriptionRecursively_NoLikedTweetsInWideAndDeepTree() {
		// Arrange
		Tweet root = new() {
			Id = 1, Likes = 0,
			Replies = new List<Tweet>() {
				new() {
					Id = 2, Likes = 0,
					Replies = new List<Tweet>() {
						new() {
							Id = 3, Likes = 0,
							Replies = new List<Tweet>() {}
						},
						new() {
							Id = 10, Likes = 0,
							Replies = new List<Tweet>() {}
						},
					},
				},
				new() {
					Id = 4, Likes = 0,
					Replies = new List<Tweet>() {}
				},
				new() {
					Id = 5, Likes = 0,
					Replies = new List<Tweet>() {
						new() {
							Id = 7, Likes = 0,
							Replies = new List<Tweet>() {
								new() {
									Id = 8, Likes = 0,
									Replies = new List<Tweet>() {}
								},
								new() {
									Id = 9, Likes = 0,
									Replies = new List<Tweet>() {}
								},
							}
						},
					}
				},
				new() {
					Id = 6, Likes = 0,
					Replies = new List<Tweet>() {}
				},
			},
		};
		IDictionary<int, (int, int)> expectedLikesAndRepliesDict = new Dictionary<int, (int, int)>() {
			{1, (0, 9)},
			{2, (0, 2)},
			{3, (0, 0)},
			{4, (0, 0)},
			{5, (0, 3)},
			{6, (0, 0)},
			{7, (0, 2)},
			{8, (0, 0)},
			{9, (0, 0)},
			{10, (0, 0)},
		};
		var expectedTotalChildrenNumber = 9;

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
	public void TestGetTweetTreeDescriptionRecursively_FullyLikedTweetsInWideAndShallowTree() {
		// Arrange
		Tweet root = new() {
			Id = 1, Likes = 1,
			Replies = new List<Tweet>() {
				new() { Id = 2, Likes = 2, Replies = new List<Tweet>() { } },
				new() { Id = 3, Likes = 4, Replies = new List<Tweet>() { } },
				new() { Id = 4, Likes = 8, Replies = new List<Tweet>() { } },
				new() { Id = 5, Likes = 16, Replies = new List<Tweet>() { } },
				new() { Id = 6, Likes = 32, Replies = new List<Tweet>() { } },
				new() { Id = 7, Likes = 64, Replies = new List<Tweet>() { } },
				new() { Id = 8, Likes = 128, Replies = new List<Tweet>() { } },
				new() { Id = 9, Likes = 256, Replies = new List<Tweet>() { } },
				new() { Id = 10, Likes = 512, Replies = new List<Tweet>() { } },
				new() { Id = 11, Likes = 1024, Replies = new List<Tweet>() { } },
				new() { Id = 12, Likes = 2048, Replies = new List<Tweet>() { } },
				new() { Id = 13, Likes = 4096, Replies = new List<Tweet>() { } },
				new() { Id = 14, Likes = 8192, Replies = new List<Tweet>() { } },
				new() { Id = 15, Likes = 16384, Replies = new List<Tweet>() { } },
				new() { Id = 16, Likes = 32768, Replies = new List<Tweet>() { } },
				new() { Id = 17, Likes = 65536, Replies = new List<Tweet>() { } },
			},
		};
		IDictionary<int, (int, int)> expectedLikesAndRepliesDict = new Dictionary<int, (int, int)>() {
			{1, (1, 16)},
			{2, (2, 0)},
			{3, (4, 0)},
			{4, (8, 0)},
			{5, (16, 0)},
			{6, (32, 0)},
			{7, (64, 0)},
			{8, (128, 0)},
			{9, (256, 0)},
			{10, (512, 0)},
			{11, (1024, 0)},
			{12, (2048, 0)},
			{13, (4096, 0)},
			{14, (8192, 0)},
			{15, (16384, 0)},
			{16, (32768, 0)},
			{17, (65536, 0)},
		};
		var expectedTotalChildrenNumber = 16;

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
	public void TestCalculateTweetScore_VeryRecentAndRelevantTweet() {
		// Arrange
		DateTime currentDate = new(2023, 12, 7, 12, 0, 0),
				 tweetDate = new(2023, 12, 7, 12, 0, 1);
		var likesAndReplies = (5000, 1000);

		int expectedScore = 5000 * 100 + 1000 * 70 - 0;

		// Act
		int actualScore = CalculateTweetScore(likesAndReplies, currentDate, tweetDate);

		// Assert
		Assert.Equal(expectedScore, actualScore);
	}

	[Fact]
	public void TestCalculateTweetScore_MostIrrelevantTweet() {
		// Arrange
		DateTime currentDate = new(2023, 12, 7, 12, 0, 0),
				 tweetDate = new(2023, 10, 7, 12, 0, 1);
		var likesAndReplies = (0, 0);

		int expectedScore = -24 * 3;

		// Act
		int actualScore = CalculateTweetScore(likesAndReplies, currentDate, tweetDate);

		// Assert
		Assert.Equal(expectedScore, actualScore);
	}

	[Fact]
	public void TestCalculateTweetScore_PrettyRecentButIrrelevantTweet() {
		// Arrange
		DateTime currentDate = new(2023, 12, 7, 12, 0, 0),
				 tweetDate = new(2023, 12, 7, 12, 0, 1);
		var likesAndReplies = (0, 0);

		int expectedScore = 0;

		// Act
		int actualScore = CalculateTweetScore(likesAndReplies, currentDate, tweetDate);

		// Assert
		Assert.Equal(expectedScore, actualScore);
	}

	[Fact]
	public void TestCalculateTweetScore_NotRecentAndNotFamousTweet() {
		// Arrange
		DateTime currentDate = new(2023, 12, 7, 12, 0, 0),
				 tweetDate = new(2023, 12, 7, 5, 0, 1);
		var likesAndReplies = (15, 1);

		int expectedScore = 15 * 100 + 1 * 70 - 7 * 3;

		// Act
		int actualScore = CalculateTweetScore(likesAndReplies, currentDate, tweetDate);

		// Assert
		Assert.Equal(expectedScore, actualScore);
	}

	[Fact]
	public void TestCalculateTweetScore_NotRecentAndWithNoLikesTweet() {
		// Arrange
		DateTime currentDate = new(2023, 12, 7, 12, 0, 0),
				 tweetDate = new(2023, 12, 7, 4, 0, 1);
		var likesAndReplies = (0, 4);

		int expectedScore = 4 * 70 - 8 * 3;

		// Act
		int actualScore = CalculateTweetScore(likesAndReplies, currentDate, tweetDate);

		// Assert
		Assert.Equal(expectedScore, actualScore);
	}

	[Fact]
	public void TestCalculateTweetScore_NotRecentAndWithNoRepliesTweet() {
		// Arrange
		DateTime currentDate = new(2023, 12, 7, 12, 0, 0),
				 tweetDate = new(2023, 12, 7, 0, 0, 1);
		var likesAndReplies = (3, 0);

		int expectedScore = 3 * 100 - 12 * 3;

		// Act
		int actualScore = CalculateTweetScore(likesAndReplies, currentDate, tweetDate);

		// Assert
		Assert.Equal(expectedScore, actualScore);
	}

	[Fact]
	public void TestCalculateTweetScore_NotRecentButStillRelevantTweet() {
		// Arrange
		DateTime currentDate = new(2023, 12, 7, 12, 0, 0),
				 tweetDate = new(2023, 10, 7, 0, 0, 1);
		var likesAndReplies = (376, 26);

		int expectedScore = 376 * 100 + 26 * 70 - 24 * 3;

		// Act
		int actualScore = CalculateTweetScore(likesAndReplies, currentDate, tweetDate);

		// Assert
		Assert.Equal(expectedScore, actualScore);
	}

	[Fact]
	public void TestCalculateTweetScore_MiddleTermRelevantTweet() {
		// Arrange
		DateTime currentDate = new(2023, 12, 7, 12, 0, 0),
				 tweetDate = new(2023, 12, 7, 8, 0, 1);
		var likesAndReplies = (44, 6);

		int expectedScore = 44 * 100 + 6 * 70 - 4 * 3;

		// Act
		int actualScore = CalculateTweetScore(likesAndReplies, currentDate, tweetDate);

		// Assert
		Assert.Equal(expectedScore, actualScore);
	}
}

