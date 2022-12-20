using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using totten_romatoes.Server.Services;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReviewModelsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IDropboxService _dropboxService;
        private readonly IImageService _imageService;

        public ReviewModelsController(IReviewService reviewService, IDropboxService dropboxService, IImageService imageService)
        {
            _reviewService = reviewService;
            _dropboxService = dropboxService;
            _imageService = imageService;
        }

        // GET: api/ReviewModels
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<ReviewModel> GetReviews()
        {
            return _reviewService.GetAllReviews();
        }

        // GET: api/ReviewModels/5
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

        // PUT: api/ReviewModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /* [HttpPut("{id}")]
         public async Task<IActionResult> PutReviewModel(int id, ReviewModel reviewModel)
         {
             if (id != reviewModel.Id)
             {
                 return BadRequest();
             }

             _context.Entry(reviewModel).State = EntityState.Modified;

             try
             {
                 await _context.SaveChangesAsync();
             }
             catch (DbUpdateConcurrencyException)
             {
                 if (!ReviewModelExists(id))
                 {
                     return NotFound();
                 }
                 else
                 {
                     throw;
                 }
             }

             return NoContent();
         }*/

        // POST: api/ReviewModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReviewModel>> PostReviewModel(ReviewModel reviewModel)
                      {
                if (reviewModel.ReviewImage != null)
            {
                string imageUrlOnDropbox = await _dropboxService.UploadImageToDropbox(reviewModel.ReviewImage);
                reviewModel.ReviewImage.ImageUrl = imageUrlOnDropbox;
                await _imageService.SaveImageToDb(reviewModel.ReviewImage);
                Console.WriteLine(reviewModel.ReviewImage.Id);
            }
            _reviewService.AddReviewToDb(reviewModel);
            return CreatedAtAction("GetReviewModel", new { id = reviewModel.Id }, reviewModel);
        }

        // DELETE: api/ReviewModels/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReviewModel(int id)
        {
            var reviewModel = await _context.Reviews.FindAsync(id);
            if (reviewModel == null)
            {
                return NotFound();
            }
            _context.Reviews.Remove(reviewModel);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ReviewModelExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }*/
    }
}
