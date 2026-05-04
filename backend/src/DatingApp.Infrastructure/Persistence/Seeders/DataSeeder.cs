using DatingApp.Domain.Entities;
using DatingApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DatingApp.Infrastructure.Persistence.Seeders;

public class DataSeeder(AppDbContext db, ILogger<DataSeeder> logger)
{
    public async Task SeedAsync()
    {
        if (await db.Users.AnyAsync())
        {
            logger.LogInformation("Database already seeded — skipping.");
            return;
        }

        logger.LogInformation("Seeding database with test data...");

        var users = GenerateUsers();
        await db.Users.AddRangeAsync(users);
        await db.SaveChangesAsync();

        await SeedSwipesAndMatchesAsync(users);

        logger.LogInformation("Seeding completed. {Count} users created.", users.Count);
    }

    private static List<User> GenerateUsers()
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Test1234!");

        return
        [
            new User
            {
                Id = Guid.NewGuid(),
                Email = "alice@test.com",
                PasswordHash = passwordHash,
                FirstName = "Alice",
                LastName = "Martin",
                DateOfBirth = new DateOnly(1997, 4, 15),
                Gender = Gender.Female,
                Bio = "Passionnée de voyage et de photographie 📸 Toujours partante pour une bonne aventure !",
                City = "Paris",
                Country = "France",
                Latitude = 48.8566,
                Longitude = 2.3522,
                IsActive = true,
                GenderPreferences = [Gender.Male],
                MinAgePreference = 25,
                MaxAgePreference = 35,
                MaxDistanceKm = 50,
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow,
                LastActiveAt = DateTime.UtcNow,
                Photos =
                [
                    new Photo
                    {
                        Id = Guid.NewGuid(),
                        Url = "https://randomuser.me/api/portraits/women/44.jpg",
                        PublicId = "seed/alice_1",
                        IsMain = true,
                        Order = 1
                    }
                ]
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "bob@test.com",
                PasswordHash = passwordHash,
                FirstName = "Bob",
                LastName = "Dupont",
                DateOfBirth = new DateOnly(1994, 8, 22),
                Gender = Gender.Male,
                Bio = "Développeur le jour, guitariste la nuit 🎸 Fan de cuisine italienne.",
                City = "Lyon",
                Country = "France",
                Latitude = 45.7640,
                Longitude = 4.8357,
                IsActive = true,
                GenderPreferences = [Gender.Female],
                MinAgePreference = 22,
                MaxAgePreference = 32,
                MaxDistanceKm = 80,
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow,
                LastActiveAt = DateTime.UtcNow.AddMinutes(-30),
                Photos =
                [
                    new Photo
                    {
                        Id = Guid.NewGuid(),
                        Url = "https://randomuser.me/api/portraits/men/32.jpg",
                        PublicId = "seed/bob_1",
                        IsMain = true,
                        Order = 1
                    }
                ]
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "camille@test.com",
                PasswordHash = passwordHash,
                FirstName = "Camille",
                LastName = "Bernard",
                DateOfBirth = new DateOnly(1999, 1, 10),
                Gender = Gender.Female,
                Bio = "Architecte en devenir 🏛️ J'adore le café, les musées et les balades en vélo.",
                City = "Bordeaux",
                Country = "France",
                Latitude = 44.8378,
                Longitude = -0.5792,
                IsActive = true,
                GenderPreferences = [Gender.Male, Gender.Female],
                MinAgePreference = 23,
                MaxAgePreference = 35,
                MaxDistanceKm = 60,
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow,
                LastActiveAt = DateTime.UtcNow.AddHours(-2),
                Photos =
                [
                    new Photo
                    {
                        Id = Guid.NewGuid(),
                        Url = "https://randomuser.me/api/portraits/women/68.jpg",
                        PublicId = "seed/camille_1",
                        IsMain = true,
                        Order = 1
                    }
                ]
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "david@test.com",
                PasswordHash = passwordHash,
                FirstName = "David",
                LastName = "Leroy",
                DateOfBirth = new DateOnly(1991, 6, 5),
                Gender = Gender.Male,
                Bio = "Médecin urgentiste 🏥 Je cherche quelqu'un qui comprend les horaires décalés 😄",
                City = "Marseille",
                Country = "France",
                Latitude = 43.2965,
                Longitude = 5.3698,
                IsActive = true,
                GenderPreferences = [Gender.Female],
                MinAgePreference = 27,
                MaxAgePreference = 38,
                MaxDistanceKm = 40,
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow,
                LastActiveAt = DateTime.UtcNow.AddHours(-5),
                Photos =
                [
                    new Photo
                    {
                        Id = Guid.NewGuid(),
                        Url = "https://randomuser.me/api/portraits/men/55.jpg",
                        PublicId = "seed/david_1",
                        IsMain = true,
                        Order = 1
                    }
                ]
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "emma@test.com",
                PasswordHash = passwordHash,
                FirstName = "Emma",
                LastName = "Rousseau",
                DateOfBirth = new DateOnly(1996, 11, 28),
                Gender = Gender.Female,
                Bio = "Prof de yoga 🧘 Végétarienne convaincue, amoureuse des chats et du soleil.",
                City = "Nice",
                Country = "France",
                Latitude = 43.7102,
                Longitude = 7.2620,
                IsActive = true,
                GenderPreferences = [Gender.Male],
                MinAgePreference = 28,
                MaxAgePreference = 40,
                MaxDistanceKm = 30,
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow,
                LastActiveAt = DateTime.UtcNow.AddMinutes(-10),
                Photos =
                [
                    new Photo
                    {
                        Id = Guid.NewGuid(),
                        Url = "https://randomuser.me/api/portraits/women/12.jpg",
                        PublicId = "seed/emma_1",
                        IsMain = true,
                        Order = 1
                    }
                ]
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@test.com",
                PasswordHash = passwordHash,
                FirstName = "Admin",
                LastName = "System",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Gender = Gender.Male,
                Bio = "Compte administrateur.",
                City = "Paris",
                Country = "France",
                Latitude = 48.8566,
                Longitude = 2.3522,
                IsActive = true,
                GenderPreferences = [],
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow,
                LastActiveAt = DateTime.UtcNow,
                Photos = []
            }
        ];
    }

    private async Task SeedSwipesAndMatchesAsync(List<User> users)
    {
        // Alice ❤️ Bob → Match
        var alice = users.First(u => u.Email == "alice@test.com");
        var bob = users.First(u => u.Email == "bob@test.com");
        var camille = users.First(u => u.Email == "camille@test.com");
        var david = users.First(u => u.Email == "david@test.com");

        // Alice swipe Right Bob
        var swipe1 = new Swipe { Id = Guid.NewGuid(), SwiperId = alice.Id, SwipedId = bob.Id, Direction = SwipeDirection.Right, CreatedAt = DateTime.UtcNow.AddDays(-2) };
        // Bob swipe Right Alice → Match
        var swipe2 = new Swipe { Id = Guid.NewGuid(), SwiperId = bob.Id, SwipedId = alice.Id, Direction = SwipeDirection.Right, CreatedAt = DateTime.UtcNow.AddDays(-2) };

        await db.Swipes.AddRangeAsync(swipe1, swipe2);

        // Créer le match Alice ↔ Bob
        var match = new Match
        {
            Id = Guid.NewGuid(),
            User1Id = alice.Id,
            User2Id = bob.Id,
            IsActive = true,
            CreatedAt = DateTime.UtcNow.AddDays(-2)
        };
        await db.Matches.AddAsync(match);
        await db.SaveChangesAsync();

        // Ajouter des messages dans le match
        var messages = new List<Message>
        {
            new() { Id = Guid.NewGuid(), MatchId = match.Id, SenderId = alice.Id, Content = "Salut Bob ! 👋", IsRead = true, ReadAt = DateTime.UtcNow.AddDays(-1), CreatedAt = DateTime.UtcNow.AddDays(-2).AddHours(1) },
            new() { Id = Guid.NewGuid(), MatchId = match.Id, SenderId = bob.Id, Content = "Salut Alice ! Ça va ? 😊", IsRead = true, ReadAt = DateTime.UtcNow.AddDays(-1), CreatedAt = DateTime.UtcNow.AddDays(-2).AddHours(2) },
            new() { Id = Guid.NewGuid(), MatchId = match.Id, SenderId = alice.Id, Content = "Super et toi ? Tu es dev c'est ça ? 💻", IsRead = true, ReadAt = DateTime.UtcNow.AddHours(-3), CreatedAt = DateTime.UtcNow.AddDays(-1) },
            new() { Id = Guid.NewGuid(), MatchId = match.Id, SenderId = bob.Id, Content = "Oui exactement ! Et toi tu fais quoi dans la vie ?", IsRead = false, CreatedAt = DateTime.UtcNow.AddHours(-1) },
        };
        await db.Messages.AddRangeAsync(messages);

        // David swipe Right Emma (pas encore de retour)
        var swipe3 = new Swipe { Id = Guid.NewGuid(), SwiperId = david.Id, SwipedId = camille.Id, Direction = SwipeDirection.Right, CreatedAt = DateTime.UtcNow.AddHours(-3) };
        await db.Swipes.AddAsync(swipe3);

        await db.SaveChangesAsync();
        logger.LogInformation("Matches and messages seeded.");
    }
}