using Mcce22.SmartOffice.Management.Managers;
using Mcce22.SmartOffice.Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlideshowItemController : ControllerBase
    {
        private readonly ISlideshowItemManager _slideshowItemManager;

        public SlideshowItemController(ISlideshowItemManager slideshowManager)
        {
            _slideshowItemManager = slideshowManager;
        }

        [HttpGet]
        public Task<SlideshowItemModel[]> GetSlideshowItems(int userId)
        {
            return _slideshowItemManager.GetSlideshowItems(userId);
        }

        [HttpPost]
        public async Task<SlideshowItemModel> CreateSlideshowItem([FromBody] SaveSlideshowItemModel model)
        {
            return await _slideshowItemManager.CreateSlideshowItem(model);
        }

        [HttpPost("{slideshowItemId}/content")]
        public async Task<SlideshowItemModel> StoreSlideshowItemContent(int slideshowItemId)
        {
            return await _slideshowItemManager.StoreSlideshowItemContent(slideshowItemId, Request.Body);
        }

        [HttpDelete("{slideshowItemId}")]
        public async Task DeleteSlideshowItem(int slideshowItemId)
        {
            await _slideshowItemManager.DeleteSlideshowItem(slideshowItemId);
        }
    }
}
