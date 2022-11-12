using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using ModelsDb;
using Services.Filters;
using Services.Validators;
using FluentValidation;

namespace Services
{
    public class AnnouncementService
    {
        private BulletinBoardContext _context { get; set; }

        private AnnouncementValidator _validationRules { get; set; }

        private IMapper _mapper { get; set; }

        MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public AnnouncementService()
        {
            _context = new BulletinBoardContext();
            _mapper = mapperConfiguration.CreateMapper();
            _validationRules = new AnnouncementValidator();
        }

        public async Task<Announcement> GetAnnouncementAsync(Guid id)
        {
            var announcementDb = await _context.Ads.FirstOrDefaultAsync(p => p.Id == id);

            if (announcementDb == null)
                throw new KeyNotFoundException("Такого объявления нет в базе данных"); ;

            return _mapper.Map<Announcement>(announcementDb);
        }

        public async Task AddAnnouncementAsync(Announcement announcement)
        {
            await _validationRules.ValidateAndThrowAsync(announcement);

            var userDb = await _context.Users.FirstOrDefaultAsync(p => p.Id == announcement.UserId);

            if (userDb == null)
                throw new KeyNotFoundException("Такого пользователя нет в базе данных");           

            if (await _context.Ads.FirstOrDefaultAsync(p => p.Id == announcement.Id) != null)
                throw new ArgumentException("Такое объявление уже существует");

            var announcementDb = _mapper.Map<AnnouncementDb>(announcement);
            _context.Ads.Add(announcementDb);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnnouncementAsync(Announcement announcement)
        {
            await _validationRules.ValidateAndThrowAsync(announcement);

            var announcementDb = await _context.Ads.FirstOrDefaultAsync(p => p.Id == announcement.Id && p.UserId == announcement.UserId);

            if (announcementDb == null)
                throw new KeyNotFoundException("Такого объявления нет в базе данных");

            _context.Ads.Remove(announcementDb);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAnnouncementAsync(Announcement announcement)
        {
            await _validationRules.ValidateAndThrowAsync(announcement);

            var announcementDb = await _context.Ads.FirstOrDefaultAsync(p => p.Id == announcement.Id && p.UserId == announcement.UserId);

            if (announcementDb == null)
                throw new KeyNotFoundException("Такого объявления нет в базе данных");

            announcementDb.Id = announcement.Id;
            announcementDb.Number = announcement.Number;
            announcementDb.UserId = announcement.UserId;
            announcementDb.Text = announcement.Text;
            announcementDb.Image = announcement.Image;
            announcementDb.Rating = announcement.Rating;
            announcementDb.Created = announcement.Created;
            announcementDb.ExpirationDate = announcement.ExpirationDate;

            _context.Ads.Update(announcementDb);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Announcement>> GetFilteredAnnouncements(AnnouncementFilter filter)
        {
            var query = _context.Ads.AsQueryable().AsNoTracking();

            if (filter.Number != 0)
                query = query.Where(p => p.Number == filter.Number);

            if (filter.UserId != default)
                query = query.Where(p => p.UserId == filter.UserId);

            if (filter.Text != null)
                query = query.Where(p => p.Text == filter.Text);

            if (filter.Image != null)
                query = query.Where(p => p.Image == filter.Image);

            if (filter.maxRating != 0)
                query = query.Where(p => p.Rating <= filter.maxRating);

            if (filter.minRating != 0)
                query = query.Where(p => p.Rating >= filter.minRating);

            if (filter.StartDate != default)
                query = query.Where(p => p.Created >= filter.StartDate);

            if (filter.EndDate != default)
                query = query.Where(p => p.Created <= filter.EndDate);

            var filteredList = await query.ToListAsync();

            List<Announcement> ads = _mapper.Map<List<Announcement>>(filteredList);

            return ads;
        }
    }
}
