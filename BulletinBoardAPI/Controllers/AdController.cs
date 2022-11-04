using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Models;
using ModelsDb;
using Services;
using Services.Filters;
using System.IO;

namespace BulletinBoardAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdController : ControllerBase
    {
        private AdService _service { get; set; }

        private BulletinBoardContext _context { get; set; }

        public AdController(IWebHostEnvironment webHostEnvironment)
        {
            _service = new AdService();
            _context = new BulletinBoardContext();
        }

        [HttpGet]
        public async Task<ActionResult<List<Ad>>> GetFilteredAds([FromQuery] AdFilter filter)
        {
            
            var filteredAds = await _service.GetFilteredAds(filter);

            if (filteredAds.Count != 0)
            {
                return filteredAds;
            }

            return new NotFoundResult();
        }

        [HttpGet("image")]
        public async Task<IActionResult> GetImage([FromQuery] Guid id)
        {
            try
            {
                var ad = await _service.GetAdAsync(id);

                if (System.IO.File.Exists(ad.Image))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(ad.Image);
                    return File(bytes, "image/png");
                }

                return new NotFoundResult();
            }
            catch(KeyNotFoundException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddAdAsync([FromQuery] Ad ad)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == ad.UserId) == null)
                return new BadRequestObjectResult(new KeyNotFoundException("Такого клиента нет в базе данных"));

            if (_context.Ads.FirstOrDefault(u => u.Id == ad.Id) != null)
                return new BadRequestObjectResult(new KeyNotFoundException("Такое объявление уже сущетвует"));

            await _service.AddAdAsync(ad);
            return Ok();

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAdAsync([FromQuery] Ad ad)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == ad.UserId) == null)
                return new BadRequestObjectResult(new KeyNotFoundException("Такого клиента нет в базе данных"));

            if (_context.Ads.FirstOrDefault(u => u.Id == ad.Id) == null)
                return new BadRequestObjectResult(new KeyNotFoundException("Такое объявление уже сущетвует"));

            await _service.DeleteAdAsync(ad);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAdAsync([FromQuery] Ad ad)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == ad.UserId) == null)
                return new BadRequestObjectResult(new KeyNotFoundException("Такого клиента нет в базе данных"));

            if (_context.Ads.FirstOrDefault(u => u.Id == ad.Id) == null)
                return new BadRequestObjectResult(new KeyNotFoundException("Такое объявление уже сущетвует"));

            await _service.UpdateAdAsync(ad);
            return Ok();
        }
    }
}
