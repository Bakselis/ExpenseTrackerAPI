

using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Domain;

namespace ExpenseTracker.Services
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllTagsAsync();

        Task<bool> CreateTagAsync(Tag tag);
        
        Task<bool> DeleteTagAsync(string tagName);
    }
}