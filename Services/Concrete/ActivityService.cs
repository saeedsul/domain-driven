using Persistence.Entities;
using Persistence;
using Services.Interfaces;
using Common.Dtos.Activity;
using Microsoft.EntityFrameworkCore;

namespace Services.Concrete
{
    public class ActivityService : IActivityService
    {
        private readonly IApplicationDbContext _context;

        public ActivityService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActivityDto?> Create(AddActivityDto addActivityDto)
        {
            if (addActivityDto == null ||
                string.IsNullOrWhiteSpace(addActivityDto.Name) ||
                string.IsNullOrWhiteSpace(addActivityDto.FromAddress) ||
                string.IsNullOrWhiteSpace(addActivityDto.ToEmailAddress) ||
                string.IsNullOrWhiteSpace(addActivityDto.FromName))
            {
                return null;
            }

            var newActivity = new Activity
            {
                Id = Guid.NewGuid(),
                Name = addActivityDto.Name,
                FromAddress = addActivityDto.FromAddress,
                ToEmailAddress = addActivityDto.ToEmailAddress,
                FromName = addActivityDto.FromName,
                CreatedDate = DateTime.UtcNow,
                SentDate = DateTime.UtcNow
            };

            _context.Activities.Add(newActivity);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ? MapToDto(newActivity) : null;
        }

        public async Task<bool> Delete(Guid id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return false;

            _context.Activities.Remove(activity);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<ActivityDto>> GetAll()
        {
            var activities = await _context.Activities.ToListAsync();
            return activities.Select(MapToDto);
        }

        public async Task<ActivityDto?> GetById(Guid id)
        {
            var activity = await _context.Activities.FindAsync(id);
            return activity == null ? null : MapToDto(activity);
        }

        public async Task<ActivityDto?> Update(Guid id, UpdateActivityDto updateDto)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return null;

            activity.Name = updateDto.Name ?? activity.Name;
            activity.FromAddress = updateDto.FromAddress ?? activity.FromAddress;
            activity.ToEmailAddress = updateDto.ToEmailAddress ?? activity.ToEmailAddress;
            activity.FromName = updateDto.FromName ?? activity.FromName;
            activity.BouncedDate = updateDto.BouncedDate;
            activity.OpenedDate = updateDto.OpenedDate;

            await _context.SaveChangesAsync();

            return MapToDto(activity);
        }         
         

        private ActivityDto MapToDto(Activity activity)
        {
            return new ActivityDto
            {
                Id = activity.Id,
                Name = activity.Name,
                FromAddress = activity.FromAddress,
                ToEmailAddress = activity.ToEmailAddress,
                FromName = activity.FromName,
                CreatedDate = activity.CreatedDate,
                SentDate = activity.SentDate,
                OpenedDate = activity.OpenedDate,
                BouncedDate = activity.BouncedDate
            };
        }
    }
}
