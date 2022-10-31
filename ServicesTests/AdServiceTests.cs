using Xunit;
using Services;
using Models;
using Services.Filters;
using Bogus;

namespace ServicesTests
{
    public class AdServiceTests
    {
        [Fact]
        public async Task AddAdAsync_AddingErrors_SuccessfulAdd_Test()
        {
            //Arrange
            UserService userService = new UserService();
            AdService adService = new AdService();
            TestDataGenerator generator = new TestDataGenerator();

            var existsUser = generator.GetFakerDataUser().Generate();
            var noExistsUser = generator.GetFakerDataUser().Generate();

            var adExistsUser = generator.GetFakerDataAd(existsUser.Id).Generate();
            var adNoExistsUser = generator.GetFakerDataAd(noExistsUser.Id).Generate();

            //Act/Assert
            try
            {
                await userService.AddUserAsync(existsUser);
                await adService.AddAdAsync(adExistsUser);

                Assert.NotNull(await adService.GetAdAsync(adExistsUser.Id));
                await Assert.ThrowsAsync<KeyNotFoundException>(() => adService.AddAdAsync(adNoExistsUser));
                await Assert.ThrowsAsync<ArgumentException>(() => adService.AddAdAsync(adExistsUser));            
            
            }
            catch(Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public async Task DeleteAdAsync_DeletionErrors_SuccessfulDelete_Test()
        {
            //Arrange
            UserService userService = new UserService();
            AdService adService = new AdService();
            TestDataGenerator generator = new TestDataGenerator();

            var existsUser = generator.GetFakerDataUser().Generate();
            var noExistsUser = generator.GetFakerDataUser().Generate();

            var existsAd = generator.GetFakerDataAd(existsUser.Id).Generate();
            var noExistsAd = generator.GetFakerDataAd(existsUser.Id).Generate();
            var adNoExistsUser = generator.GetFakerDataAd(noExistsUser.Id).Generate();

            //Act/Assert
            try
            {
                await userService.AddUserAsync(existsUser);
                await adService.AddAdAsync(existsAd);
                await adService.DeleteAdAsync(existsAd);

                await Assert.ThrowsAsync<KeyNotFoundException>(() => adService.GetAdAsync(existsAd.Id));
                await Assert.ThrowsAsync<KeyNotFoundException>(() => adService.DeleteAdAsync(adNoExistsUser));
                await Assert.ThrowsAsync<KeyNotFoundException>(() => adService.DeleteAdAsync(noExistsAd));
               
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public async Task UpdateAdAsync_UpdateErrors_SuccessfulUpdate_Test()
        {
            //Arrange
            UserService userService = new UserService();
            AdService adService = new AdService();
            TestDataGenerator generator = new TestDataGenerator();

            var existsUser = generator.GetFakerDataUser().Generate();
            var noExistsUser = generator.GetFakerDataUser().Generate();

            var existsAd = generator.GetFakerDataAd(existsUser.Id).Generate();
            var noExistsAd = generator.GetFakerDataAd(existsUser.Id).Generate();
            var adNoExistsUser = generator.GetFakerDataAd(noExistsUser.Id).Generate();

            //Act/Assert
            try
            {
                await userService.AddUserAsync(existsUser);
                await adService.AddAdAsync(existsAd);
                existsAd.Rating = 7;
                await adService.UpdateAdAsync(existsAd);

                Assert.Equal(existsAd.Rating, adService.GetAdAsync(existsAd.Id).Result.Rating);
                await Assert.ThrowsAsync<KeyNotFoundException>(() => adService.UpdateAdAsync(adNoExistsUser));
                await Assert.ThrowsAsync<KeyNotFoundException>(() => adService.UpdateAdAsync(noExistsAd));

            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }
}
}
