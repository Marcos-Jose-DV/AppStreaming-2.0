

using Models;
using Models.Movies;

namespace Repository.Interfaces;

public interface  IAssessmentsRepository
{
    Task PostAll(IEnumerable<Assessments> assessments);
    Task<List<Assessments>> GetAllAssessments();
    Task<IEnumerable<CardHome>> GetCardsHome(int page = 0);
    Task<IEnumerable<CardHome>> GetFilterAsync(string filter);
    Task<IEnumerable<CardHome>> GetNameAsync(string name);
    Task<Assessments> GetByIdAsync(int id);
    Task PostAsync(Assessments assessment);
    Task<Assessments> UpdateAsync(Assessments assessment);
    Task DeleteAsync(int id);
}
