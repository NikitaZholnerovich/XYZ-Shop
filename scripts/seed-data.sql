/*
  XYZ-Shop sample seed data (INSERT only).
  Run AFTER applying EF Core migrations (dotnet ef database update).

  Demo accounts (BCrypt hashes):
    admin  / Admin123!  (Role = 99 Admin)
    user1  / User123!   (Role = 1  User)
    mod1   / Mod123!    (Role = 10 Moderator)

  Example:
    sqlcmd -S localhost,1433 -U sa -P "Your_strong_Password1" -d XYZ-project -i scripts/seed-data.sql
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF EXISTS (SELECT 1 FROM dbo.Games)
BEGIN
    PRINT 'Seed skipped: Games table already has data.';
    RETURN;
END;

BEGIN TRANSACTION;

DECLARE @Now datetime2 = SYSUTCDATETIME();

-- Publishers
SET IDENTITY_INSERT dbo.Publishers ON;
INSERT INTO dbo.Publishers (Id, Name, Description, CreatedAt, ModifiedAt) VALUES
(1, N'Nova Interactive', N'Indie studio focused on atmospheric adventures.', @Now, NULL),
(2, N'Iron Peak Games', N'Makers of competitive multiplayer titles.', @Now, NULL),
(3, N'Pixel Harbor', N'Casual and family-friendly experiences.', @Now, NULL);
SET IDENTITY_INSERT dbo.Publishers OFF;

-- Genres
SET IDENTITY_INSERT dbo.GameGenres ON;
INSERT INTO dbo.GameGenres (Id, Name, CreatedAt, ModifiedAt) VALUES
(1, N'Action', @Now, NULL),
(2, N'RPG', @Now, NULL),
(3, N'Strategy', @Now, NULL),
(4, N'Adventure', @Now, NULL),
(5, N'Indie', @Now, NULL);
SET IDENTITY_INSERT dbo.GameGenres OFF;

-- User profiles
SET IDENTITY_INSERT dbo.UserProfiles ON;
INSERT INTO dbo.UserProfiles (Id, Email, FirstName, LastName, Mobilephone, BirthDate, CreatedAt, ModifiedAt) VALUES
(1, N'admin@xyz-shop.local', N'Alex', N'Admin', N'+10000000001', '1990-01-15', @Now, NULL),
(2, N'user1@xyz-shop.local', N'Sam', N'Player', N'+10000000002', '1995-06-20', @Now, NULL),
(3, N'mod1@xyz-shop.local', N'Morgan', N'Moderator', N'+10000000003', '1992-11-03', @Now, NULL);
SET IDENTITY_INSERT dbo.UserProfiles OFF;

-- Users (Language: 1 = English; Role: 1 User, 10 Moderator, 99 Admin)
SET IDENTITY_INSERT dbo.Users ON;
INSERT INTO dbo.Users (Id, Login, PasswordHash, Role, Language, AvatarUrl, UserProfileId, CreatedAt, ModifiedAt) VALUES
(1, N'admin', N'$2a$11$eyxvoMqS7oM37gNB8EzKaewkgJr50Af7/O/Xnfk5dcI9ABEAcJCuS', 99, 1, NULL, 1, @Now, NULL),
(2, N'user1', N'$2a$11$CgOfhTDED7FpiSe7Nddp1.Pqyt25UL0VH/WjLw6l4KMO2.6LgGd52', 1, 1, NULL, 2, @Now, NULL),
(3, N'mod1', N'$2a$11$sJ0l6Uc9Bcb0hrVo/ok7Eerwwdmr1KRXSrHcnE9.uAC76W/1Zjp7O', 10, 2, NULL, 3, @Now, NULL);
SET IDENTITY_INSERT dbo.Users OFF;

-- Games
SET IDENTITY_INSERT dbo.Games ON;
INSERT INTO dbo.Games (
    Id, Title, Description, ImageUrl, PublisherId, CreatedByUserId, ModifiedByUserId,
    Price, AverageRating, ReviewsCount, PositiveReviewsCount, CreatedAt, ModifiedAt
) VALUES
(1, N'Echoes of Ember', N'Explore a burning archipelago and restore the ancient flame.',
    N'https://cdn.pixabay.com/photo/2017/08/10/02/05/tiles-2617112_1280.jpg',
    1, 1, 1, 29.99, 9.0, 2, 2, @Now, NULL),
(2, N'Iron Siege', N'Lead your clan in real-time multiplayer sieges.',
    N'https://cdn.pixabay.com/photo/2016/11/29/09/32/architecture-1868667_1280.jpg',
    2, 1, 1, 39.99, 7.0, 1, 1, @Now, NULL),
(3, N'Harbor Tales', N'A cozy fishing and crafting adventure by the sea.',
    N'https://cdn.pixabay.com/photo/2016/11/29/05/45/astronomy-1867616_1280.jpg',
    3, 1, NULL, 14.99, 8.0, 1, 1, @Now, NULL),
(4, N'Neon Drift', N'Arcade racing through neon megacities at night.',
    N'https://cdn.pixabay.com/photo/2016/11/18/17/46/architecture-1836070_1280.jpg',
    2, 1, 1, 19.99, NULL, 0, 0, @Now, NULL),
(5, N'Crystal Depths', N'Dungeon crawl RPG with procedural caves and relics.',
    N'https://cdn.pixabay.com/photo/2017/01/18/16/46/hong-kong-1990268_1280.jpg',
    1, 1, NULL, 24.99, 10.0, 1, 1, @Now, NULL),
(6, N'Quiet Meadows', N'Relaxing farm life sim with seasonal festivals.',
    N'https://cdn.pixabay.com/photo/2015/12/01/20/28/road-1072823_1280.jpg',
    3, 3, 3, 9.99, NULL, 0, 0, @Now, NULL),
(7, N'Star Cartographers', N'Chart unknown systems in this turn-based strategy epic.',
    N'https://cdn.pixabay.com/photo/2016/10/20/18/35/earth-1756274_1280.jpg',
    2, 1, NULL, 34.99, 6.0, 1, 0, @Now, NULL),
(8, N'Paper Knights', N'Hand-drawn indie tactics with origami warriors.',
    N'https://cdn.pixabay.com/photo/2014/09/07/21/52/city-438393_1280.jpg',
    1, 3, NULL, 12.49, NULL, 0, 0, @Now, NULL);
SET IDENTITY_INSERT dbo.Games OFF;

-- Game <-> Genre
INSERT INTO dbo.GamesToGenres (GameGenresId, GamesId) VALUES
(4, 1), (5, 1),
(1, 2), (3, 2),
(4, 3), (5, 3),
(1, 4),
(2, 5), (1, 5),
(4, 6), (5, 6),
(3, 7),
(3, 8), (5, 8);

-- Reviews (Rating 1-10)
SET IDENTITY_INSERT dbo.GameReviews ON;
INSERT INTO dbo.GameReviews (Id, Text, Rating, AuthorId, GameId, CreatedAt, ModifiedAt) VALUES
(1, N'Beautiful world and memorable soundtrack. Highly recommended.', 9, 2, 1, @Now, NULL),
(2, N'Great exploration; a few puzzles felt unfair late-game.', 9, 3, 1, @Now, NULL),
(3, N'Solid PvP, needs more maps.', 7, 2, 2, @Now, NULL),
(4, N'Perfect wind-down game after work.', 8, 2, 3, @Now, NULL),
(5, N'Best dungeon crawler I played this year.', 10, 3, 5, @Now, NULL),
(6, N'Interesting systems but the UI is cluttered.', 6, 2, 7, @Now, NULL);
SET IDENTITY_INSERT dbo.GameReviews OFF;

-- Community chat
SET IDENTITY_INSERT dbo.CommunityChatMessages ON;
INSERT INTO dbo.CommunityChatMessages (Id, MessageText, UserId, CreatedAt, ModifiedAt) VALUES
(1, N'Welcome to XYZ-Shop community chat!', 1, @Now, NULL),
(2, N'Anyone tried Echoes of Ember yet?', 2, DATEADD(MINUTE, 1, @Now), NULL),
(3, N'Yes — great atmosphere. Looking forward to DLC.', 3, DATEADD(MINUTE, 2, @Now), NULL);
SET IDENTITY_INSERT dbo.CommunityChatMessages OFF;

-- Friends: user1 <-> mod1
-- MyFriendsId = friend being added; WhoIsMyFriendsId = owner of the friend list
INSERT INTO dbo.UserEntityUserEntity (MyFriendsId, WhoIsMyFriendsId) VALUES
(3, 2),
(2, 3);

COMMIT TRANSACTION;

PRINT 'Seed completed successfully.';
