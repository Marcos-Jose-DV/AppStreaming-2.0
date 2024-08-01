

using Models;
using Models.Movies;

namespace Repository.Interfaces;

public interface  IAssessmentsRepository
{
    Task PostAll(Assessments assessments);
    Task<IEnumerable<CardHome>> GetAllAsync();
    Task<IEnumerable<CardHome>> GetFilterAsync(string filter);
    Task<Assessments> GetByIdAsync(int id);
    Task PostAsync(Assessments assessment);
    Task<Assessments> UpdateAsync(Assessments assessment);
    Task DeleteAsync(int id);
}
