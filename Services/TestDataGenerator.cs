using Bogus;
using Models;

namespace Services
{
    public class TestDataGenerator
    {
        public Faker<User> GetFakerDataUser()
        {
            var generator = new Faker<User>()
                .StrictMode(true)
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.IsAdmin, f => f.Random.Bool());

            return generator;
        }
    }
}
