using Models;
using Services;
using Xunit;

namespace ServicesTests
{
    public class ValidatorTests
    {
        [Fact]
        public async Task AdValidator_ErrorWhenWriting_IncorrectData_Test()
        {
            //Arange
            UserService userService = new UserService();
            AnnouncementService adService = new AnnouncementService();
            TestDataGenerator generator = new TestDataGenerator();

            Announcement emptyAnnouncement = new Announcement();

            Announcement negativeRating = new Announcement
            {
                Id = Guid.NewGuid(),
                Number = 1,
                UserId = Guid.NewGuid(),
                Text = "Text",
                Rating = -5,
                Created = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(7)
            };

            //Act//Assert
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => adService.AddAnnouncementAsync(emptyAnnouncement));
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => adService.AddAnnouncementAsync(negativeRating));
        }

        [Fact]
        public async Task UserValidator_ErrorWhenWriting_IncorrectData_Test()
        {
            //Arrange
            UserService userService = new UserService();

            User emptyUser = new User();

            User wrongUsername = new User
            {
                Id = new Guid(),
                Name = "1234",
                IsAdmin = false

            };

            User correctUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "Tomas Hamer",
                IsAdmin = false
            };

            //Act//Assert
            await userService.AddUserAsync(correctUser);
            Assert.NotNull(await userService.GetUserAsync(correctUser.Id));

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => userService.AddUserAsync(emptyUser));
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => userService.AddUserAsync(wrongUsername));

        }
    }
}
