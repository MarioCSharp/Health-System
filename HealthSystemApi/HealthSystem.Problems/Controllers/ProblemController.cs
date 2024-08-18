using HealthSystem.Problems.Models;
using HealthSystem.Problems.Services.ProblemService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.Problems.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProblemController : ControllerBase
    {
        private IProblemService problemService;

        public ProblemController(IProblemService problemService)
        {
            this.problemService = problemService;
        }

        [HttpGet("Add")]
        public async Task<IActionResult> Add([FromQuery] ProblemAddModel problemAddModel, [FromQuery] SymptomAddModel symptoms, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await problemService.AddAsync(problemAddModel, symptoms.SymptomIds, userId);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet("Remove")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            var result = await problemService.RemoveAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            var result = await problemService.DetailsAsync(id);

            return Ok(result);
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit([FromQuery] ProblemEditModel healthIssueEditModel, [FromQuery] SymptomAddModel symptoms)
        {
            var result = await problemService.EditAsync(healthIssueEditModel, symptoms.SymptomIds);

            return Ok(result);
        }

        [HttpGet("UserIssues")]
        public async Task<IActionResult> UserIssues(string userId)
        {
            var result = await problemService.UserProblemsAsync(userId);

            return Ok(result);
        }

        [HttpGet("AddSymptoms")]
        public async Task<IActionResult> AddSymptoms()
        {
            await problemService.AddSymptomsAsync();

            return Ok();
        }

        [HttpGet("GetSymptomCategories")]
        public async Task<IActionResult> GetSymptomCategories()
        {
            return Ok(await problemService.LoadCategoriesForMAUI());
        }

        [HttpGet("GetSymptomSubCategories")]
        public async Task<IActionResult> GetSymptomSubCategories()
        {
            return Ok(await problemService.LoadSubCategoriesForMAUI());
        }
    }
}
