using Microsoft.EntityFrameworkCore;
using Onion.Assignment.Services.Greetings.Data.DataSources;
using Onion.Assignment.Services.Greetings.Data.Models;
using Onion.Assignment.Services.Greetings.Domain.Entities;
using Onion.Assignment.Services.Greetings.Domain.Interfaces;
using Onion.Assignment.Services.Greetings.Domain.ValueObjects;

namespace Onion.Assignment.Services.Greetings.Data.Repositories;

public class SqliteGreetingRepository : IGreetingRepository
{
    private readonly GreetingDbContext _context;

    public SqliteGreetingRepository(GreetingDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Greeting> SaveAsync(Greeting greeting, CancellationToken cancellationToken = default)
    {
        var dataModel = MapToDataModel(greeting);
        
        await _context.Greetings.AddAsync(dataModel, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return greeting;
    }

    public async Task<Greeting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _context.Greetings
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            
        return dataModel != null ? MapToDomainEntity(dataModel) : null;
    }

    public async Task<IEnumerable<Greeting>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var dataModels = await _context.Greetings
            .OrderBy(g => g.CreatedAt)
            .ToListAsync(cancellationToken);
            
        return dataModels.Select(MapToDomainEntity);
    }

    public async Task<IEnumerable<Greeting>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var dataModels = await _context.Greetings
            .Where(g => g.Name.ToLower() == name.ToLower())
            .OrderBy(g => g.CreatedAt)
            .ToListAsync(cancellationToken);
            
        return dataModels.Select(MapToDomainEntity);
    }

    private static GreetingDataModel MapToDataModel(Greeting greeting)
    {
        return new GreetingDataModel
        {
            Id = greeting.Id,
            Name = greeting.Name.Value,
            Message = greeting.Message,
            CreatedAt = greeting.CreatedAt
        };
    }

    private static Greeting MapToDomainEntity(GreetingDataModel dataModel)
    {
        var personName = new PersonName(dataModel.Name);
        
        return Greeting.Reconstruct(
            dataModel.Id,
            personName,
            dataModel.Message,
            dataModel.CreatedAt
        );
    }
}