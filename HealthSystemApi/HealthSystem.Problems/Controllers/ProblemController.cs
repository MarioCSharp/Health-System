using HealthSystem.Problems.Models;
using HealthSystem.Problems.Services.ProblemService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.Problems.Controllers
{
    /// <summary>
    /// Controller responsible for handling problem-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProblemController : ControllerBase
    {
        private IProblemService problemService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProblemController"/> class.
        /// </summary>
        /// <param name="problemService">Service to handle problem-related operations.</param>
        public ProblemController(IProblemService problemService)
        {
            this.problemService = problemService;
        }

        /// <summary>
        /// Adds a new problem with associated symptoms.
        /// </summary>
        /// <param name="problemAddModel">Model containing problem details to be added.</param>
        /// <param name="symptoms">Model containing the list of symptom IDs to associate with the problem.</param>
        /// <param name="userId">ID of the user associated with the problem.</param>
        /// <returns>Result of the add operation.</returns>
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

        /// <summary>
        /// Removes a problem by its ID.
        /// </summary>
        /// <param name="id">ID of the problem to be removed.</param>
        /// <returns>Result of the remove operation.</returns>
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

        /// <summary>
        /// Retrieves the details of a problem by its ID.
        /// </summary>
        /// <param name="id">ID of the problem to retrieve details for.</param>
        /// <returns>Details of the problem.</returns>
        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            var result = await problemService.DetailsAsync(id);

            return Ok(result);
        }

        /// <summary>
        /// Edits an existing problem and updates its associated symptoms.
        /// </summary>
        /// <param name="healthIssueEditModel">Model containing updated problem details.</param>
        /// <param name="symptoms">Model containing updated list of symptom IDs to associate with the problem.</param>
        /// <returns>Result of the edit operation.</returns>
        [HttpGet("Edit")]
        public async Task<IActionResult> Edit([FromQuery] ProblemEditModel healthIssueEditModel, [FromQuery] SymptomAddModel symptoms)
        {
            var result = await problemService.EditAsync(healthIssueEditModel, symptoms.SymptomIds);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves the list of problems for a specified user.
        /// </summary>
        /// <param name="userId">ID of the user whose problems are to be retrieved.</param>
        /// <returns>List of user's problems.</returns>
        [HttpGet("UserIssues")]
        public async Task<IActionResult> UserIssues(string userId)
        {
            var result = await problemService.UserProblemsAsync(userId);

            return Ok(result);
        }

        /// <summary>
        /// Adds new symptoms to the system.
        /// </summary>
        /// <returns>Result of the add operation.</returns>
        [HttpGet("AddSymptoms")]
        public async Task<IActionResult> AddSymptoms()
        {
            await problemService.AddSymptomsAsync();

            return Ok();
        }

        /// <summary>
        /// Retrieves the list of symptom categories.
        /// </summary>
        /// <returns>List of symptom categories.</returns>
        [HttpGet("GetSymptomCategories")]
        public async Task<IActionResult> GetSymptomCategories()
        {
            return Ok(await problemService.LoadCategoriesForMAUI());
        }

        /// <summary>
        /// Retrieves the list of symptom subcategories.
        /// </summary>
        /// <returns>List of symptom subcategories.</returns>
        [HttpGet("GetSymptomSubCategories")]
        public async Task<IActionResult> GetSymptomSubCategories()
        {
            return Ok(await problemService.LoadSubCategoriesForMAUI());
        }
    }
}
