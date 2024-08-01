using DataBase.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Movies;
using Repository.Interfaces;

namespace Repository.Repositoreis;

public class AssessmentsRepository : IAssessmentsRepository
{
    private readonly AppDbContext _appDbContext;

    public AssessmentsRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }


    public async Task<IEnumerable<CardHome>> GetAllAsync()
    {
        var assessments = await _appDbContext.Assessments
        .Select(c => new CardHome(c.Id, c.ImagePath))
        .AsNoTracking()
        .ToListAsync();

        return CardHome.GetCardsHome(assessments);
    }

    public async Task<Assessments> GetByIdAsync(int id)
    {
        var assessment = await _appDbContext.Assessments
            .FirstOrDefaultAsync(x => x.Id == id);

        return assessment;
    }

    public async Task PostAll(Assessments assessments)
    {
        await _appDbContext.Assessments.AddAsync(assessments);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task PostAsync(Assessments assessment)
    {
        assessment.LastUpdate = DateTime.UtcNow;
        await _appDbContext.Assessments.AddAsync(assessment);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<Assessments> UpdateAsync(Assessments assessment)
    {
        var newValue = await _appDbContext.Assessments
            .FirstOrDefaultAsync(x => x.Id == assessment.Id);

        if (newValue is not null)
        {
            newValue.Name = assessment.Name;
            newValue.Assessment = assessment.Assessment;
            newValue.Director = assessment.Director;
            newValue.ImagePath = assessment.ImagePath;
            newValue.Gender = assessment.Gender;
            newValue.Duration = assessment.Duration;
            newValue.Position = assessment.Position;
            newValue.Concluded = assessment.Concluded;
            newValue.Comments = assessment.Comments;
            newValue.Category = assessment.Category;
            newValue.LastUpdate = DateTime.Now;
            newValue.Launch = assessment.Launch;

            _appDbContext.Update(newValue);
            await _appDbContext.SaveChangesAsync();
        }

        return newValue;
    }

    public async Task DeleteAsync(int id)
    {
        var assessment = _appDbContext.Assessments
            .FirstOrDefault(x => x.Id == id);

        _appDbContext.Assessments.Remove(assessment);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<CardHome>> GetFilterAsync(string filter)
    {
        var assessments = await _appDbContext.Assessments
             .Where(x => x.Category == filter)
             .Select(x => new CardHome(x.Id, x.ImagePath))
             .AsNoTracking()
             .ToListAsync();

        return CardHome.GetCardsHome(assessments);
    }
}
