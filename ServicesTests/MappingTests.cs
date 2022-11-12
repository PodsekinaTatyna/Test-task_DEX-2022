using Xunit;
using Services;
using Models;
using ModelsDb;
using AutoMapper;

namespace ServicesTests
{
    public class MappingTests
    {
        [Fact]
        public void СheckForSuccessfulMapping_Ad_And_User_Test()
        {
            //Arrange
            var mapperConfiguration = new MapperConfiguration(p =>
            {
                p.AddProfile<AppMappingProfile>();
            });

            var mapper = mapperConfiguration.CreateMapper();

            User user = new User
            {
                Id = new Guid(),
                Name = "Petr",
                IsAdmin = true,
            };

            Announcement ad = new Announcement
            {
                Id = new Guid(),
                Number = 1,
                UserId = user.Id,
                Text = "Text",
                Image = "image",
                Rating = 10,
                Created = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(7),
            };

            //At
            var userDb = mapper.Map<UserDb>(user);
            var adDb = mapper.Map<AnnouncementDb>(ad);

            //Assert
            Assert.Equal(typeof(UserDb), userDb.GetType());
            Assert.Equal(typeof(AnnouncementDb), adDb.GetType());

        }

        [Fact]
        public void СheckForSuccessfulMapping_AdDb_And_UserDb_Test()
        {
            //Arrange
            var mapperConfiguration = new MapperConfiguration(p =>
            {
                p.AddProfile<AppMappingProfile>();
            });

            var mapper = mapperConfiguration.CreateMapper();

            UserDb userDb = new UserDb
            {
                Id = new Guid(),
                Name = "Petr",
                IsAdmin = true,
            };

            AnnouncementDb adDb = new AnnouncementDb
            {
                Id = new Guid(),
                Number = 1,
                UserId = userDb.Id,
                Text = "Text",
                Image = "image",
                Rating = 10,
                Created = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(7),
            };

            //At
            var user = mapper.Map<User>(userDb);
            var ad = mapper.Map<Announcement>(adDb);

            //Assert
            Assert.Equal(typeof(User), user.GetType());
            Assert.Equal(typeof(Announcement), ad.GetType());

        }
    }
}
