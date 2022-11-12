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
    public class AnnouncementController : ControllerBase
    {
        private AnnouncementService _service { get; set; }

        public AnnouncementController()
        {
            _service = new AnnouncementService();
        }

        [HttpGet]
        public async Task<ActionResult<List<Announcement>>> GetFilteredAnnouncements([FromQuery] AnnouncementFilter filter)
        {

            var filteredAds = await _service.GetFilteredAnnouncements(filter);

            if (filteredAds.Count != 0)
            {
                return filteredAds;
            }

            return new NotFoundResult();
        }


        [HttpPost]
        public async Task<IActionResult> AddAnnouncementAsync([FromBody] Announcement announcement)
        {
            try
            {
                await _service.AddAnnouncementAsync(announcement);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAnnouncementAsync([FromQuery] Announcement announcement)
        {
            try
            {
                await _service.DeleteAnnouncementAsync(announcement);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAnnouncementAsync([FromBody] Announcement announcement)
        {
            try
            {
                await _service.UpdateAnnouncementAsync(announcement);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
