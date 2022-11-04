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
        public async Task AddUserAsync_AddingErrors_SuccessfulAdd_Test()
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
        public async Task DeleteUserAsync_DeletionErrors_SuccessfulDelete_Test()
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
        public async Task UpdateUserAsync_UpdateErrors_SuccessfulUpdate_Test()
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
        public async Task GetFilteredUsers_OrdinaryUsers_Test()
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
            foreach(User user in users)
            {
                Assert.True(user.IsAdmin == userFilter.IsAdmin);
            }

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
            try
            {
                await userService.AddUserAsync(correctUser);
                Assert.NotNull(await userService.GetUserAsync(correctUser.Id));

                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => userService.AddUserAsync(emptyUser));
                await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => userService.AddUserAsync(wrongUsername));
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }
    }
}
