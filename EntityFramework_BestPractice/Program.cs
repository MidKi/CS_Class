using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

Console.WriteLine("Ce projet utilise le projet EntityFrameowrk_BestPractice_DataAccessLibrary");

var factory = new PeopleContextFactory();
using var dbContext = factory.CreateDbContext();

Query1(dbContext);

async Task LoadSampleData(PeopleContext _db)
{
    if(_db.People.Count() == 0)
    {
        string file = await File.ReadAllTextAsync("generated.json");
        var people = JsonSerializer.Deserialize<List<Person>>(file);

        await _db.AddRangeAsync(people);

        await _db.SaveChangesAsync();
        Console.WriteLine("people added");
    }
}

void Query1(PeopleContext _db)
{
    var people = _db.People
        .Include(a => a.Addresses)
        .Include(a => a.EmailAddresses)
        .ToList();
}