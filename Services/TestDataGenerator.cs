﻿using Bogus;
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

        public Faker<Ad> GetFakerDataAd(Guid userId)
        {
            var generator = new Faker<Ad>()
                .StrictMode(true)
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.Number, f => f.Random.Int(1, 10))
                .RuleFor(u => u.UserId, f => userId)
                .RuleFor(u => u.Text, f => f.Lorem.Text())
                .RuleFor(u => u.Image, f => "C:\\Users\\Professional\\Desktop\\Test-task_DEX-2022\\Image\\" + f.Random.Int(1,3).ToString() + ".jpg")
                .RuleFor(u => u.Rating, f => f.Random.Int(1, 10))
                .RuleFor(u => u.CreatedBy, f => DateTime.Now.Date)
                .RuleFor(u => u.ExpirationDate, f => DateTime.Now.AddDays(7).Date);

            return generator;

        }
    }
}
