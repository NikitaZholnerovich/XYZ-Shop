/*
  XYZ-Shop sample seed data (popular Steam titles).
  Run AFTER EF Core migrations (dotnet ef database update).

  Re-runnable: clears catalog/demo rows, then inserts fresh data.
  Image URLs use Steam CDN:
    https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/{appId}/header.jpg

  Demo accounts (BCrypt):
    admin  / Admin123!  (Role = 99 Admin)
    user1  / User123!   (Role = 1  User)
    mod1   / Mod123!    (Role = 10 Moderator)

  Example:
    sqlcmd -S localhost,1433 -U sa -P 'YourPassword' -C -I -d XYZ-project -i scripts/seed-data.sql
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

BEGIN TRANSACTION;

-- Clear existing sample data (FK-safe order)
DELETE FROM dbo.GamesToGenres;
DELETE FROM dbo.GameReviews;
DELETE FROM dbo.CommunityChatMessages;
DELETE FROM dbo.UserEntityUserEntity;
DELETE FROM dbo.Games;
DELETE FROM dbo.Users;
DELETE FROM dbo.UserProfiles;
DELETE FROM dbo.GameGenres;
DELETE FROM dbo.Publishers;

DECLARE @Now datetime2 = SYSUTCDATETIME();

-- Publishers
SET IDENTITY_INSERT dbo.Publishers ON;
INSERT INTO dbo.Publishers (Id, Name, Description, CreatedAt, ModifiedAt) VALUES
(1,  N'Larian Studios', N'Belgian RPG studio behind Baldur''s Gate 3 and Divinity: Original Sin.', @Now, NULL),
(2,  N'FromSoftware', N'Creators of Elden Ring, Dark Souls, and Sekiro.', @Now, NULL),
(3,  N'CD PROJEKT RED', N'Polish studio known for The Witcher and Cyberpunk 2077.', @Now, NULL),
(4,  N'Rockstar Games', N'Open-world action publisher of GTA and Red Dead.', @Now, NULL),
(5,  N'Valve', N'Platform holder and developer of Counter-Strike, Dota, Half-Life.', @Now, NULL),
(6,  N'Warner Bros. Games', N'Publisher of Hogwarts Legacy and other licensed titles.', @Now, NULL),
(7,  N'Supergiant Games', N'Indie studio behind Hades, Bastion, and Transistor.', @Now, NULL),
(8,  N'ConcernedApe', N'Independent developer of Stardew Valley.', @Now, NULL),
(9,  N'Game Science', N'Developer of Black Myth: Wukong.', @Now, NULL),
(10, N'Sony Interactive Entertainment', N'PlayStation publishing label (God of War and more).', @Now, NULL),
(11, N'Team Cherry', N'Australian indie studio, creators of Hollow Knight.', @Now, NULL),
(12, N'Re-Logic', N'Indie studio behind Terraria.', @Now, NULL),
(13, N'Bethesda Softworks', N'Publisher of The Elder Scrolls, Fallout, and DOOM.', @Now, NULL),
(14, N'Capcom', N'Japanese publisher of Monster Hunter, Resident Evil, and Street Fighter.', @Now, NULL),
(15, N'Electronic Arts', N'Major publisher of FIFA, Apex Legends, and It Takes Two.', @Now, NULL),
(16, N'ZA/UM', N'Studio behind Disco Elysium.', @Now, NULL),
(17, N'Maddy Makes Games', N'Indie studio known for Celeste.', @Now, NULL),
(18, N'Zeekerss', N'Independent developer of Lethal Company.', @Now, NULL);
SET IDENTITY_INSERT dbo.Publishers OFF;

-- Genres
SET IDENTITY_INSERT dbo.GameGenres ON;
INSERT INTO dbo.GameGenres (Id, Name, CreatedAt, ModifiedAt) VALUES
(1, N'Action', @Now, NULL),
(2, N'RPG', @Now, NULL),
(3, N'Adventure', @Now, NULL),
(4, N'Strategy', @Now, NULL),
(5, N'Indie', @Now, NULL),
(6, N'Shooter', @Now, NULL),
(7, N'Simulation', @Now, NULL),
(8, N'Horror', @Now, NULL);
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

-- Popular games (Steam header art)
SET IDENTITY_INSERT dbo.Games ON;
INSERT INTO dbo.Games (
    Id, Title, Description, ImageUrl, PublisherId, CreatedByUserId, ModifiedByUserId,
    Price, AverageRating, ReviewsCount, PositiveReviewsCount, CreatedAt, ModifiedAt
) VALUES
(1, N'Baldur''s Gate 3',
 N'Story-rich party-based RPG set in the Dungeons & Dragons universe. Your choices shape a tale of fellowship, betrayal, and absolute power.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1086940/header.jpg',
 1, 1, 1, 59.99, 9.5, 2, 2, @Now, NULL),

(2, N'ELDEN RING',
 N'Rise, Tarnished, and be guided by grace to brandish the power of the Elden Ring and become an Elden Lord in the Lands Between.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1245620/header.jpg',
 2, 1, 1, 59.99, 9.0, 1, 1, @Now, NULL),

(3, N'Cyberpunk 2077',
 N'An open-world action-adventure RPG set in Night City, a megalopolis obsessed with power, glamour, and body modification.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1091500/header.jpg',
 3, 1, NULL, 59.99, 8.0, 1, 1, @Now, NULL),

(4, N'The Witcher 3: Wild Hunt',
 N'Play as Geralt of Rivia, a monster hunter for hire, in a vast open world full of merchant cities, pirate islands, and dangerous mountain passes.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/292030/header.jpg',
 3, 1, 1, 39.99, 10.0, 1, 1, @Now, NULL),

(5, N'Grand Theft Auto V',
 N'Experience the sprawling story of Michael, Franklin, and Trevor in Los Santos — one of the most acclaimed open-world games ever made.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/271590/header.jpg',
 4, 1, NULL, 29.99, 8.5, 1, 1, @Now, NULL),

(6, N'Red Dead Redemption 2',
 N'America, 1899. Arthur Morgan and the Van der Linde gang are outlawed and on the run in this epic Western adventure.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1174180/header.jpg',
 4, 1, 1, 59.99, 9.0, 1, 1, @Now, NULL),

(7, N'Counter-Strike 2',
 N'The legendary competitive FPS returns with upgraded Source 2 visuals, smoke gameplay, and ranked matchmaking.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/730/header.jpg',
 5, 1, NULL, 0.00, 8.0, 1, 1, @Now, NULL),

(8, N'Hogwarts Legacy',
 N'Experience life as a student at Hogwarts in the 1800s. Your character is a key part of an ancient secret that threatens the wizarding world.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/990080/header.jpg',
 6, 1, NULL, 59.99, 7.5, 1, 1, @Now, NULL),

(9, N'Hades',
 N'Defy the god of the dead as you hack and slash your way out of the Underworld in this rogue-like dungeon crawler from Supergiant Games.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1145360/header.jpg',
 7, 3, 3, 24.99, 9.5, 1, 1, @Now, NULL),

(10, N'Stardew Valley',
 N'You''ve inherited your grandfather''s old farm plot. With a few tools and some coins, you set out to build the farm of your dreams.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/413150/header.jpg',
 8, 3, NULL, 14.99, 9.0, 1, 1, @Now, NULL),

(11, N'Black Myth: Wukong',
 N'An action RPG rooted in Chinese mythology. You shall set out as the Destined One to venture into realms inspired by Journey to the West.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/2358720/header.jpg',
 9, 1, 1, 59.99, 8.5, 1, 1, @Now, NULL),

(12, N'God of War (2018)',
 N'His vengeance against the Gods of Olympus behind him, Kratos now lives in the realm of Norse gods and monsters with his son Atreus.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1593500/header.jpg',
 10, 1, NULL, 49.99, 9.5, 1, 1, @Now, NULL),

(13, N'Hollow Knight',
 N'Forge your path in Hallownest as you explore winding caverns, battle corrupted bugs, and solve ancient mysteries.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/367520/header.jpg',
 11, 3, NULL, 14.99, 9.0, 1, 1, @Now, NULL),

(14, N'Sekiro: Shadows Die Twice',
 N'Carve your own clever path to vengeance in an epic adventure from FromSoftware, creators of Bloodborne and the Dark Souls series.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/814380/header.jpg',
 2, 1, NULL, 59.99, 9.0, 1, 1, @Now, NULL),

(15, N'Dota 2',
 N'Every day, millions of players worldwide enter battle as one of over a hundred Dota heroes in this free-to-play MOBA.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/570/header.jpg',
 5, 1, NULL, 0.00, 8.5, 1, 1, @Now, NULL),

(16, N'Portal 2',
 N'The cake is still a lie. Solve mind-bending puzzles with portals, lasers, and gel in Valve''s acclaimed co-op and single-player sequel.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/620/header.jpg',
 5, 1, 1, 9.99, 10.0, 1, 1, @Now, NULL),

(17, N'DARK SOULS III',
 N'The fire fades and the lords go without thrones. Face punishing combat and cryptic lore in the final chapter of the Dark Souls trilogy.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/374320/header.jpg',
 2, 1, NULL, 39.99, 9.0, 1, 1, @Now, NULL),

(18, N'Terraria',
 N'Dig, fight, explore, build: the very world is at your disposal as you fight for survival, fortune, and glory in this 2D sandbox adventure.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/105600/header.jpg',
 12, 3, NULL, 9.99, 9.5, 1, 1, @Now, NULL),

(19, N'The Elder Scrolls V: Skyrim Special Edition',
 N'Winner of more than 200 Game of the Year awards — Skyrim Special Edition brings the epic fantasy to life with remastered visuals.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/489830/header.jpg',
 13, 1, 1, 39.99, 9.0, 1, 1, @Now, NULL),

(20, N'DOOM Eternal',
 N'Hell''s armies have invaded Earth. Become the Slayer in an essential combination of speed and power in the ultimate DOOM game.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/782330/header.jpg',
 13, 1, NULL, 19.99, 9.0, 1, 1, @Now, NULL),

(21, N'Monster Hunter: World',
 N'Hunt gigantic monsters in living, breathing ecosystems as you carve gear from trophies and climb the ranks of the Hunter''s Guild.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/582010/header.jpg',
 14, 1, NULL, 29.99, 8.5, 1, 1, @Now, NULL),

(22, N'It Takes Two',
 N'Embark on the craziest journey of your life in this split-screen co-op adventure about relationship strife and platforming mayhem.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1426210/header.jpg',
 15, 2, 2, 39.99, 9.5, 1, 1, @Now, NULL),

(23, N'Resident Evil 4',
 N'Revisit the nightmare that revolutionized survival horror — Leon S. Kennedy''s mission to rescue the president''s daughter, remade.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/2050650/header.jpg',
 14, 1, NULL, 39.99, 9.5, 1, 1, @Now, NULL),

(24, N'Disco Elysium - The Final Cut',
 N'Detective RPG. Highest ever metacritic for an RPG. Free speech. Absolute democracy. Disco Elysium is a groundbreaking open-world RPG.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/632470/header.jpg',
 16, 3, 3, 39.99, 10.0, 1, 1, @Now, NULL),

(25, N'Celeste',
 N'Help Madeline survive her inner demons on her journey to the top of Celeste Mountain in this tight, challenging platformer.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/504230/header.jpg',
 17, 3, NULL, 19.99, 9.5, 1, 1, @Now, NULL),

(26, N'Lethal Company',
 N'A co-op horror about scavenging scrap for the Company. Stay alive, meet the quota, and try not to scream.',
 N'https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1966720/header.jpg',
 18, 2, NULL, 9.99, 9.0, 1, 1, @Now, NULL);
SET IDENTITY_INSERT dbo.Games OFF;

-- Game <-> Genre
INSERT INTO dbo.GamesToGenres (GameGenresId, GamesId) VALUES
(2, 1), (3, 1), (4, 1),   -- BG3: RPG, Adventure, Strategy
(1, 2), (2, 2),             -- Elden Ring: Action, RPG
(1, 3), (2, 3),             -- Cyberpunk: Action, RPG
(2, 4), (3, 4),             -- Witcher 3: RPG, Adventure
(1, 5), (3, 5),             -- GTA V: Action, Adventure
(1, 6), (3, 6),             -- RDR2: Action, Adventure
(1, 7), (6, 7),             -- CS2: Action, Shooter
(2, 8), (3, 8),             -- Hogwarts: RPG, Adventure
(1, 9), (5, 9),             -- Hades: Action, Indie
(5, 10), (7, 10),           -- Stardew: Indie, Simulation
(1, 11), (2, 11),           -- Wukong: Action, RPG
(1, 12), (3, 12),           -- GoW: Action, Adventure
(1, 13), (5, 13),           -- Hollow Knight: Action, Indie
(1, 14), (3, 14),           -- Sekiro: Action, Adventure
(1, 15), (4, 15),           -- Dota 2: Action, Strategy
(3, 16), (5, 16),           -- Portal 2: Adventure, Indie
(1, 17), (2, 17),           -- DS3: Action, RPG
(3, 18), (5, 18),           -- Terraria: Adventure, Indie
(2, 19), (3, 19),           -- Skyrim: RPG, Adventure
(1, 20), (6, 20),           -- DOOM Eternal: Action, Shooter
(1, 21), (3, 21),           -- MHW: Action, Adventure
(1, 22), (3, 22),           -- It Takes Two: Action, Adventure
(1, 23), (8, 23),           -- RE4: Action, Horror
(2, 24), (3, 24),           -- Disco Elysium: RPG, Adventure
(3, 25), (5, 25),           -- Celeste: Adventure, Indie
(5, 26), (8, 26);           -- Lethal Company: Indie, Horror

-- Reviews
SET IDENTITY_INSERT dbo.GameReviews ON;
INSERT INTO dbo.GameReviews (Id, Text, Rating, AuthorId, GameId, CreatedAt, ModifiedAt) VALUES
(1,  N'Masterpiece of choice-driven RPG design. Combat and writing are outstanding.', 10, 2, 1, @Now, NULL),
(2,  N'Hundreds of hours of content. Act 3 still has a few rough edges, but worth it.', 9, 3, 1, @Now, NULL),
(3,  N'FromSoftware peak. Exploration never gets old.', 9, 2, 2, @Now, NULL),
(4,  N'Phantom Liberty and the 2.0 update transformed the game.', 8, 2, 3, @Now, NULL),
(5,  N'Still the gold standard for open-world storytelling.', 10, 3, 4, @Now, NULL),
(6,  N'GTA Online keeps me coming back years later.', 8, 2, 5, DATEADD(MINUTE, 1, @Now), NULL),
(7,  N'Incredible world and characters. A must-play Western.', 9, 3, 6, @Now, NULL),
(8,  N'Competitive FPS that still defines the genre.', 8, 2, 7, @Now, NULL),
(9,  N'Beautiful Hogwarts exploration; combat gets repetitive late-game.', 7, 2, 8, @Now, NULL),
(10, N'Perfect rogue-like loop with god-tier writing.', 10, 3, 9, @Now, NULL),
(11, N'Cozy, deep, and endlessly replayable.', 9, 2, 10, @Now, NULL),
(12, N'Stunning boss fights and spectacle.', 8, 3, 11, @Now, NULL),
(13, N'Best father-son story in gaming.', 10, 2, 12, @Now, NULL),
(14, N'Metroivania perfection. Atmosphere is unmatched.', 9, 3, 13, @Now, NULL),
(15, N'Demanding combat that feels amazing once it clicks.', 9, 2, 14, @Now, NULL),
(16, N'Still the best free MOBA after all these years.', 8, 2, 15, @Now, NULL),
(17, N'Puzzle design and humor are unmatched. Play co-op!', 10, 3, 16, @Now, NULL),
(18, N'Brutal, fair, and gorgeous. Bonfire ahead.', 9, 2, 17, @Now, NULL),
(19, N'Endless crafting and bosses. Perfect with friends.', 10, 3, 18, @Now, NULL),
(20, N'Mods keep Skyrim immortal. Still discovering quests.', 9, 2, 19, @Now, NULL),
(21, N'Rip and tear never felt this fast.', 9, 3, 20, @Now, NULL),
(22, N'Hunt loop is addictive once you learn the monsters.', 8, 2, 21, @Now, NULL),
(23, N'Best co-op experience I''ve played with my partner.', 10, 3, 22, @Now, NULL),
(24, N'Remake elevates a classic. Tension every encounter.', 10, 2, 23, @Now, NULL),
(25, N'Writing so good it rewired how I see RPGs.', 10, 3, 24, @Now, NULL),
(26, N'Short, precise, emotional. Assist mode is welcome.', 10, 2, 25, @Now, NULL),
(27, N'Scary, funny, and perfect in a four-stack.', 9, 3, 26, @Now, NULL);
SET IDENTITY_INSERT dbo.GameReviews OFF;

-- Community chat
SET IDENTITY_INSERT dbo.CommunityChatMessages ON;
INSERT INTO dbo.CommunityChatMessages (Id, MessageText, UserId, CreatedAt, ModifiedAt) VALUES
(1, N'Welcome to XYZ-Shop! What are you playing this week?', 1, @Now, NULL),
(2, N'Just finished Baldur''s Gate 3 — incredible.', 2, DATEADD(MINUTE, 1, @Now), NULL),
(3, N'Elden Ring DLC next. Wish me luck.', 3, DATEADD(MINUTE, 2, @Now), NULL);
SET IDENTITY_INSERT dbo.CommunityChatMessages OFF;

-- Friends: user1 <-> mod1
INSERT INTO dbo.UserEntityUserEntity (MyFriendsId, WhoIsMyFriendsId) VALUES
(3, 2),
(2, 3);

COMMIT TRANSACTION;

PRINT 'Seed completed successfully (26 popular Steam titles; Featured = games after first 15).';
