using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using ModelsDb;

namespace Services
{
    public class AdService
    {
        private BulletinBoardContext _context { get; set; }

        private IMapper _mapper { get; set; }

        MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public AdService()
        {
            _context = new BulletinBoardContext();
            _mapper = mapperConfiguration.CreateMapper();
        }

        public async Task<Ad> GetAdAsync(Guid id)
        {
            var adDb = await _context.Ads.FirstOrDefaultAsync(p => p.Id == id);

            if (adDb == null)
                throw new KeyNotFoundException("Такого объявления нет в базе данных"); ;

            return _mapper.Map<Ad>(adDb);
        }

        public async Task AddAdAsync(Ad ad)
        {
            var userDb = await _context.Users.FirstOrDefaultAsync(p => p.Id == ad.UserId);

            if (userDb == null)
                throw new KeyNotFoundException("Такого пользователя нет в базе данных");

            var adDb = _mapper.Map<AdDb>(ad);

            if (await _context.Ads.FirstOrDefaultAsync(p => p.Id == ad.Id) != null)
                throw new ArgumentException("Такое объявление уже существует");

            _context.Ads.Add(adDb);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAdAsync(Ad ad)
        {
            var userDb = await _context.Users.FirstOrDefaultAsync(p => p.Id == ad.UserId);
            
            if (userDb == null)
                throw new KeyNotFoundException("Такого пользователя нет в базе данных");

            var adDb = await _context.Ads.FirstOrDefaultAsync(p => p.Id == ad.Id);

            if (adDb == null)
                throw new KeyNotFoundException("Такого объявления нет в базе данных");

            _context.Ads.Remove(adDb);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAdAsync(Ad ad)
        {
            var userDb = await _context.Users.FirstOrDefaultAsync(p => p.Id == ad.UserId);

            if (userDb == null)
                throw new KeyNotFoundException("Такого пользователя нет в базе данных");

            var adDb = await _context.Ads.FirstOrDefaultAsync(p => p.Id == ad.Id);

            if (adDb == null)
                throw new KeyNotFoundException("Такого объявления нет в базе данных");

            adDb.Id = ad.Id;
            adDb.Number = ad.Number;
            adDb.UserId = ad.UserId;
            adDb.Text = ad.Text;
            adDb.Image = ad.Image;
            adDb.Rating = ad.Rating;
            adDb.CreatedBy = ad.CreatedBy;
            adDb.ExpirationDate = ad.ExpirationDate;

            _context.Ads.Update(adDb);
            await _context.SaveChangesAsync();
        }


    }
}
