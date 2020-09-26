using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Data;
using ExpenseTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class TagSevice
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

        public async Task<Tag> CreateTagAsync(Tag tag)
        {
            var existingTag =
                await _dataContext.Tags.SingleOrDefaultAsync(x =>
                    x.Name == tag.Name);
            if (existingTag != null)
            {
                return existingTag;
            }
            await _dataContext.Tags.AddAsync(tag);
            await _dataContext.SaveChangesAsync();
            return tag;
        }

        public async Task<bool> DeleteTagAsync(Tag tag)
        {
            var existingTag =
                await _dataContext.Tags.SingleOrDefaultAsync(x =>
                    x.Name == tag.Name);

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