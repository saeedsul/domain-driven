using Common.Dtos.Activity;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : BaseController
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpPost]
        [Route("create-activity")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActivityDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer(AddActivityDto addActivityDto)
        {
            var result = await _activityService.Create(addActivityDto);

            return result == null ? BadRequest() : CreatedAtAction(nameof(GetActivity), new { id = result.Id }, result); // Return 400 Bad Request, otherwise return 201 Created.
        }

        [HttpGet("{id}", Name = "GetActivity")]
        [ProducesResponseType(typeof(ActivityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            var activityDto = await _activityService.GetById(id);

            return activityDto == null ? NotFound() : Ok(activityDto); // Return 404 Not Found, otherwise return 200 OK.
        }

        [HttpGet("get-all-activities", Name = "GetAlActivities")]
        [ProducesResponseType(typeof(List<ActivityDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllActivities()
        {
            return Ok(await _activityService.GetAll());
        }

        [HttpPut("{id}", Name = "UpdateActivity")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateActivityDto updateActivityDto)
        {
            return await _activityService.Update(id, updateActivityDto) == null ? NotFound() : NoContent(); // Return 404 Not Found, otherwise return 204 No Content.
        }

        [HttpDelete("{id}", Name = "DeleteActivity")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            return await _activityService.Delete(id) ? NoContent() : NotFound(); // Return 404 Not Found, otherwise return 204 No Content.
        }
    }
}
