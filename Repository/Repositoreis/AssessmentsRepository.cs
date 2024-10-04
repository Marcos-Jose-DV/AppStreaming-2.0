using Azure;
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
    public async Task<Assessments> GetByIdAsync(int id)
    {
        var assessment = await _appDbContext.Assessments
            .FirstOrDefaultAsync(x => x.Id == id);

        return assessment;
    }
    public async Task<List<Assessments>> GetAllAssessments()
        => await _appDbContext.Assessments
            .OrderBy(x => x.LastUpdate)
            .ToListAsync();
    public async Task<IEnumerable<CardHome>> GetCardsHome(int page = 1)
    {
        var assessments = await _appDbContext.Assessments
          .OrderByDescending(d => d.LastUpdate)
        .Select(c => new CardHome(c.Id, c.ImagePath, c.Assessment) { Concluded = c.Concluded})
        .Skip((page - 1) * 20)
        .Take(20)
        .AsNoTracking()
        .ToListAsync();

        return assessments;
    }
    public async Task PostAll(IEnumerable<Assessments> assessments)
    {
        await _appDbContext.Assessments.AddRangeAsync(assessments);
        await _appDbContext.SaveChangesAsync();
    }
    public async Task PostAsync(Assessments assessment)
    {
        var assessmentExist = await _appDbContext.Assessments.AnyAsync(a => a.Name == assessment.Name);
        if (assessmentExist)
        {
            throw new InvalidOperationException("Essa avaliação já existe no banco de dados");
        }

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
    public async Task<IEnumerable<CardHome>> GetFilterAsync(string filter, int page = 1)
    {
        var assessments = await _appDbContext.Assessments
             .Where(x => x.Category == filter)
             .Select(x => new CardHome(x.Id, x.ImagePath, x.Assessment))
             .Skip((page - 1) * 20)
             .Take(20)
             .AsNoTracking()
             .ToListAsync();

        return CardHome.GetCardsHome(assessments);
    }
    public async Task<IEnumerable<CardHome>> GetNameAsync(string name)
    {
        var assessment = await _appDbContext.Assessments
        .Where(x => x.Name.ToLower().Contains(name.ToLower()))
        .Select(c => new CardHome(c.Id, c.ImagePath, c.Assessment))
        .AsNoTracking()
        .ToListAsync();

        if (assessment == null)
        {
            return null;
        }

        return assessment;
    }
}
