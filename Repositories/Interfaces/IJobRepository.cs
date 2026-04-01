using NearU_Backend_Revised.Models;

namespace NearU_Backend_Revised.Models;

{

    public interface IJobRepository
        {

            Task<IEnumerable<Job>> GetAllAsync();
            Task<IEnumerable<Job>> GetNewJobsAsync();
            Task<IEnumerable<Job>> GetByCategoryAsync(string category);
            Task<Job?> GetByIdAsync(string id);
            Task<Job> CreateAsync(Job job);
            Task<Job?> UpdateAsync(Job job);
            Task<bool> DeleteAsync(string id);
            Task<bool> ExistsAsync(string id);
        }

}