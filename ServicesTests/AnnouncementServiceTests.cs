using Xunit;
using Services;
using Models;
using Services.Filters;

namespace ServicesTests
{
    public class AnnouncementServiceTests
    {
        [Fact]
        public async Task AddAnnouncementAsync_AddingErrors_SuccessfulAdd_Test()
        {
            //Arrange
            UserService userService = new UserService();
            AnnouncementService announcementService = new AnnouncementService();
            TestDataGenerator generator = new TestDataGenerator();

            var existsUser = generator.GetFakerDataUser().Generate();
            var noExistsUser = generator.GetFakerDataUser().Generate();

            var announcementExistsUser = generator.GetFakerDataAnnouncement(existsUser.Id).Generate();
            var announcementNoExistsUser = generator.GetFakerDataAnnouncement(noExistsUser.Id).Generate();

            //Act/Assert
                await userService.AddUserAsync(existsUser);
                await announcementService.AddAnnouncementAsync(announcementExistsUser);

                Assert.NotNull(await announcementService.GetAnnouncementAsync(announcementExistsUser.Id));
                await Assert.ThrowsAsync<KeyNotFoundException>(() => announcementService.AddAnnouncementAsync(announcementNoExistsUser));
                await Assert.ThrowsAsync<ArgumentException>(() => announcementService.AddAnnouncementAsync(announcementExistsUser));            
            
        }

        [Fact]
        public async Task DeleteAnnouncementAsync_DeletionErrors_SuccessfulDelete_Test()
        {
            //Arrange
            UserService userService = new UserService();
            AnnouncementService announcementService = new AnnouncementService();
            TestDataGenerator generator = new TestDataGenerator();

            var existsUser = generator.GetFakerDataUser().Generate();
            var noExistsUser = generator.GetFakerDataUser().Generate();

            var existsAnnouncement = generator.GetFakerDataAnnouncement(existsUser.Id).Generate();
            var noExistsAnnouncement = generator.GetFakerDataAnnouncement(existsUser.Id).Generate();
            var announcementNoExistsUser = generator.GetFakerDataAnnouncement(noExistsUser.Id).Generate();

            //Act/Assert
            await userService.AddUserAsync(existsUser);
            await announcementService.AddAnnouncementAsync(existsAnnouncement);
            await announcementService.DeleteAnnouncementAsync(existsAnnouncement);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => announcementService.GetAnnouncementAsync(existsAnnouncement.Id));
            await Assert.ThrowsAsync<KeyNotFoundException>(() => announcementService.DeleteAnnouncementAsync(announcementNoExistsUser));
            await Assert.ThrowsAsync<KeyNotFoundException>(() => announcementService.DeleteAnnouncementAsync(noExistsAnnouncement));

        }

        [Fact]
        public async Task UpdateAnnouncementAsync_UpdateErrors_SuccessfulUpdate_Test()
        {
            //Arrange
            UserService userService = new UserService();
            AnnouncementService announcementService = new AnnouncementService();
            TestDataGenerator generator = new TestDataGenerator();

            var existsUser = generator.GetFakerDataUser().Generate();
            var noExistsUser = generator.GetFakerDataUser().Generate();

            var existsAnnouncement = generator.GetFakerDataAnnouncement(existsUser.Id).Generate();
            var noExistsAnnouncement = generator.GetFakerDataAnnouncement(existsUser.Id).Generate();
            var announcementNoExistsUser = generator.GetFakerDataAnnouncement(noExistsUser.Id).Generate();

            //Act/Assert
            await userService.AddUserAsync(existsUser);
            await announcementService.AddAnnouncementAsync(existsAnnouncement);
            existsAnnouncement.Rating = 7;
            await announcementService.UpdateAnnouncementAsync(existsAnnouncement);

            Assert.Equal(existsAnnouncement.Rating, announcementService.GetAnnouncementAsync(existsAnnouncement.Id).Result.Rating);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => announcementService.UpdateAnnouncementAsync(announcementNoExistsUser));
            await Assert.ThrowsAsync<KeyNotFoundException>(() => announcementService.UpdateAnnouncementAsync(noExistsAnnouncement));

        }

        [Fact]
        public async Task GetFilteredAnnouncements_GetAnnouncementsInDateRange_SuccessfulTest()
        {
            //Arrange
            UserService userService = new UserService();
            AnnouncementService announcementService = new AnnouncementService();
            TestDataGenerator generator = new TestDataGenerator();
            AnnouncementFilter announcementFilter = new AnnouncementFilter();

            for (int i = 0; i < 10; i++)
            {
                var user = generator.GetFakerDataUser().Generate();
                await userService.AddUserAsync(user);
                await announcementService.AddAnnouncementAsync(generator.GetFakerDataAnnouncement(user.Id).Generate());
            }

            List<Announcement> announcements = new List<Announcement>();

            //Act
            announcementFilter.Page = 1;
            announcementFilter.Size = 3;
            announcementFilter.StartDate = new DateTime(2022, 10, 20);
            announcementFilter.EndDate = new DateTime(2022, 11, 1);

            announcements = await announcementService.GetFilteredAnnouncements(announcementFilter);

            //Assert
            foreach (Announcement announcement in announcements)
            {
                Assert.True(announcement.Created >= announcementFilter.StartDate && announcement.Created <= announcementFilter.EndDate);
            }
           
        }       

    }
}
