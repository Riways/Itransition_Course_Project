using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using totten_romatoes.Server.Services;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Controllers
{
    [Route("api/reviews")]
    [Authorize]
    [ApiController]
    public class ReviewModelsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IImageService _imageService;

        public ReviewModelsController(IReviewService reviewService, IImageService imageService)
        {
            _reviewService = reviewService;
            _imageService = imageService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<ReviewModel> GetReviews()
        {
            return _reviewService.GetAllReviews();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewModel>> GetReviewModel(long id)
        {
            var reviewModel = _reviewService.GetReviewById(id);
            if (reviewModel == null)
            {
                return NotFound();
            }
            return Ok(reviewModel);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewModel>> PostReviewModel(ReviewModel reviewModel)
        {
            await _reviewService.AddReviewToDb(reviewModel);
            return CreatedAtAction("GetReviewModel", new { id = reviewModel.Id }, reviewModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReviewModel(int id)
        {
            await _reviewService.DeleteReviewDromDb(id);
            return NoContent();
        }
    }
}
