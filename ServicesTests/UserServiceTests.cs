using Xunit;
using Services;
using Models;
using Services.Filters;
using Bogus;

namespace ServicesTests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task AddUserAsync_SuccessTest()
        {
            //Arrange
            UserService userService = new UserService();
            TestDataGenerator generator = new TestDataGenerator();

            var oldUser = generator.GetFakerDataUser().Generate();
            var newUser = oldUser;

            //Act/Assert
            try
            {
                await userService.AddUserAsync(oldUser);

                await Assert.ThrowsAsync<ArgumentException>(() => userService.AddUserAsync(newUser));
                Assert.NotNull(userService.GetUserAsync(oldUser.Id));
            }
            catch(Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public async Task DeleteUserAsync()
        {
            //Arrange
            UserService userService = new UserService();
            TestDataGenerator generator = new TestDataGenerator();

            var existsUser = generator.GetFakerDataUser().Generate();
            var noExistsUser = generator.GetFakerDataUser().Generate();

            //Act/Assert
            try
            {
                await userService.AddUserAsync(existsUser);
                await userService.DeleteUserAsync(existsUser);

                await Assert.ThrowsAsync<KeyNotFoundException>(() => userService.DeleteUserAsync(noExistsUser));
                await Assert.ThrowsAsync<KeyNotFoundException>(() => userService.GetUserAsync(existsUser.Id));
            }
            catch (Exception)
            {
                Assert.True(false);
            }

        }

        [Fact]
        public async Task UpdateUserAsync()
        {
            //Arrange
            UserService userService = new UserService();
            TestDataGenerator generator = new TestDataGenerator();

            var existsUser = generator.GetFakerDataUser().Generate();
            var noExistsUser = generator.GetFakerDataUser().Generate();

            //Act/Assert
            try
            {
                await userService.AddUserAsync(existsUser);

                existsUser.Name = "New Name";
                await userService.UpdateUserAsync(existsUser);

                await Assert.ThrowsAsync<KeyNotFoundException>(() => userService.UpdateUserAsync(noExistsUser));
                Assert.Equal(existsUser.Name, userService.GetUserAsync(existsUser.Id).Result.Name);
            }
            catch(Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public async Task GetFilteredUsers()
        {
            //Arrange
            UserService userService = new UserService();
            TestDataGenerator generator = new TestDataGenerator();
            UserFilter userFilter = new UserFilter();

            for (int i = 0; i < 10; i++)
            {
                var user = generator.GetFakerDataUser().Generate();
                await userService.AddUserAsync(user);
            }

            List<User> users = new List<User>();

            //Act
            userFilter.Page = 2;
            userFilter.Size = 5;
            userFilter.IsAdmin = false;

            users = await userService.GetFilteredUsers(userFilter);

            //Assert
            Assert.NotNull(users);
        }
    }
}
