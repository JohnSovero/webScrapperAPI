using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Services;
using backend.Enums;

namespace backend.Controllers{
    [ApiController]
    [Route("scrapper")]
    [Authorize]
    public class ScrapperController(ScrapperService scrapperService) : ControllerBase {
        private readonly ScrapperService _scrapperService = scrapperService;

        // Endpoint to get scrapper data
        [HttpGet]
        public async Task<IActionResult> GetScrapper([FromQuery] string name, [FromQuery] List<PagesEnum> pages){
            if (pages == null || pages.Count < 1 || pages.Count > 3)
                return BadRequest("You must select between 1 and 3 valid pages.");

            var results = new List<object>();
            try{
                foreach (var page in pages){
                    var resultado = await _scrapperService.GetScrapperAsync(name, page.ToString());
                    results.AddRange(resultado);
                }
                return Ok(new { totalHits = results.Count, results });
            }
            catch (Exception ex){
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }
    }
}
