using Microsoft.EntityFrameworkCore;
using totten_romatoes.Server.Data;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Services
{
    public interface ISubjectService
    {
        public Task AddGradeToDb(GradeModel grade);
    }
    public class SubjectService : ISubjectService
    {
        private readonly ApplicationDbContext _dbContext;

        public SubjectService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddGradeToDb(GradeModel grade)
        {
            await _dbContext.Grades!.AddAsync(grade);
            await _dbContext.SaveChangesAsync();
        }

    }
}
