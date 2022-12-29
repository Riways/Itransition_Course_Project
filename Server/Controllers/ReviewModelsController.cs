using Duende.IdentityServer.Extensions;
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

        public ReviewModelsController(IReviewService reviewService )
        {
            _reviewService = reviewService;
        }

        [AllowAnonymous]
        [HttpGet("search/{key}")]
        public async Task<IEnumerable<ReviewModel>> FullTextSearch(string key)
        {
            if (key.IsNullOrEmpty())
                return null;
            var searchResult = await _reviewService.FullTextSearch(key);
            return searchResult;
        }

        [AllowAnonymous]
        [HttpGet("add-fakes/{amount}")]
        public async Task GenerateReviews(int amount)
        {
            if (amount < 0)
                return;
            await _reviewService.GenerateFakeReviews(amount);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<ReviewModel>> GetReviews()
        {
            return await _reviewService.GetAllReviewsWithoutComments();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewModel>> GetReviewModel(long id)
        {
            var reviewModel = await _reviewService.GetReviewById(id);
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
        [HttpPost]
        [Route("add-comment")]
        public async Task PostComment(CommentModel commentModel)
        {
            await _reviewService.AddCommentToDb(commentModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReviewModel(int id)
        {
            await _reviewService.DeleteReviewDromDb(id);
            return NoContent();
        }
    }
}
