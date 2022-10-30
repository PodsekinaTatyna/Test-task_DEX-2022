using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using ModelsDb;
using Services.Filters;
using System.Collections.Generic;

namespace Services
{
    public class UserService
    {
        private BulletinBoardContext _context { get; set; }

        private IMapper _mapper { get; set; }

        MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public UserService()
        {         
            _context = new BulletinBoardContext();
            _mapper = mapperConfiguration.CreateMapper();
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            var userDb = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

            if(userDb == null)
                throw new KeyNotFoundException("Такого пользователя нет в базе данных"); ;

            return _mapper.Map<User>(userDb);

        }

        public async Task AddUserAsync(User user)
        {
            var userDb = _mapper.Map<UserDb>(user);

            if (await _context.Users.FirstOrDefaultAsync(p => p.Id == user.Id) != null)
                throw new ArgumentException("Такой пользователь уже существует");

            _context.Users.Add(userDb);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            var userDb = await _context.Users.FirstOrDefaultAsync(p => p.Id == user.Id);

            if (userDb == null)
                throw new KeyNotFoundException("Такого пользователя нет в базе данных");

            _context.Users.Remove(userDb);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var userDb = await _context.Users.FirstOrDefaultAsync(p => p.Id == user.Id);

            if (userDb == null)
                throw new KeyNotFoundException("Такого пользователя нет в базе данных");

            userDb.Id = user.Id;
            userDb.Name = user.Name;
            userDb.IsAdmin = user.IsAdmin;

            _context.Users.Update(userDb);
            await _context.SaveChangesAsync();
        }


        public async Task<List<User>> GetFilteredUsers(UserFilter filter)
        {
            var query = _context.Users.AsQueryable().AsNoTracking();

            if (filter.Id != default)
                query = query.Where(p => p.Id == filter.Id);

            if (filter.Name != null)
                query = query.Where(p => p.Name == filter.Name);

            if (filter.IsAdmin != null)
                query = query.Where(p => p.IsAdmin == filter.IsAdmin);

            var filteredList = await query.Skip((filter.Page - 1) * filter.Size).Take(filter.Size).ToListAsync();

            var listUser = new List<User>();

            foreach(UserDb userDb in filteredList)
            {
                listUser.Add(_mapper.Map<User>(userDb));
            }

            return listUser;
        }

    }
}
