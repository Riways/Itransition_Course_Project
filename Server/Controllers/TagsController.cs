using Microsoft.AspNetCore.Mvc;
using totten_romatoes.Server.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace totten_romatoes.Server.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public TagsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        
        [HttpGet]
        [Route("take")]
        public IEnumerable<string> GetSpecificAmountOftags()
        {
            var tags = _reviewService.GetDefaultAmountOfTags();
            return tags.Select(t => t.Name).ToList();
        }

        [HttpGet]
        [Route("search/{key}")]
        public async Task<IEnumerable<string>> SearchTagsInDatabase(string key)
        {
            var tags = await _reviewService.GetListOfSimilarTagsFromDatabase(key);
            return tags.Select(t => t.Name).ToList();
        }
    }
}
