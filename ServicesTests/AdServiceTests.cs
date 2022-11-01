using Xunit;
using Services;
using Models;
using Services.Filters;
using Bogus;
using Bogus.DataSets;

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

        [Fact]
        public async Task GetFilteredAds_GetAdsInDateRange_SuccessfulTest()
        {
            //Arrange
            UserService userService = new UserService();
            AdService adService = new AdService();
            TestDataGenerator generator = new TestDataGenerator();
            AdFilter adFilter = new AdFilter();

            for (int i = 0; i < 10; i++)
            {
                var user = generator.GetFakerDataUser().Generate();
                await userService.AddUserAsync(user);
                await adService.AddAdAsync(generator.GetFakerDataAd(user.Id).Generate());
            }

            List<Ad> ads = new List<Ad>();

            //Act
            adFilter.Page = 1;
            adFilter.Size = 3;
            adFilter.StartDate = new DateTime(2022, 10, 20);
            adFilter.EndDate = new DateTime(2022, 11, 1);

            ads = await adService.GetFilteredAds(adFilter);

            //Assert
            foreach (Ad ad in ads)
            {
                Assert.True(ad.CreatedBy >= adFilter.StartDate && ad.CreatedBy <= adFilter.EndDate);
            }
           
        }
    }
}
