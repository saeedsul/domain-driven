using Common.Dtos.Activity;

namespace Services.Interfaces
{
    public interface IActivityService
    {
        Task<ActivityDto?> GetById(Guid id);
        Task<IEnumerable<ActivityDto>> GetAll();
        Task<ActivityDto?> Create(AddActivityDto addActivityDto);
        Task<ActivityDto?> Update(Guid id, UpdateActivityDto updateActivityDto);
        Task<bool> Delete(Guid id);
    }
}
