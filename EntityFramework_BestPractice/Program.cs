using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

Console.WriteLine("Ce projet utilise le projet EntityFrameowrk_BestPractice_DataAccessLibrary");

var factory = new PeopleContextFactory();
using var dbContext = factory.CreateDbContext();

//BenchmarkRunner.Run<BenchEF>();

//Query1(dbContext);
//Query2(dbContext);

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


//en général c'est mieux de faire le truc côté code mais si le serveur est vraiment cracké pk pas
[Benchmark]
void Query1(PeopleContext _db)
{
    var people = _db.People
        .Include(a => a.Addresses)
        .Include(a => a.EmailAddresses)
        .Where(x => x.Age >= 18 && x.Age <= 65) //DB qui fait le job
        .ToList();
}

void Query2(PeopleContext _db)
{
    var people = _db.People
        .Include(a => a.Addresses)
        .Include(a => a.EmailAddresses)
        .ToList()
        .Where(x => x.Age >= 18 && x.Age <= 65); //code qui fait le job
}