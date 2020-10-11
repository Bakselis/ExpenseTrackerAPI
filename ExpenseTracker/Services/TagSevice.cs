using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Data;
using ExpenseTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class TagSevice : ITagService
    {
        
        private readonly DataContext _dataContext;

        public TagSevice(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _dataContext.Tags.AsNoTracking().ToListAsync();
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            tag.Name = tag.Name.ToLower();
            var existingTag = await _dataContext.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tag.Name);
            if (existingTag != null)
                return true;

            await _dataContext.Tags.AddAsync(tag);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteTagAsync(string tagName)
        {
            var existingTag =
                await _dataContext.Tags.SingleOrDefaultAsync(x =>
                    x.Name == tagName);

            if (existingTag == null)
            {
                return false;
            }
            
            _dataContext.Tags.Remove(existingTag);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }
    }
}