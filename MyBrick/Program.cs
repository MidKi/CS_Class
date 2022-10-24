using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyBrick;
using System.ComponentModel.DataAnnotations;

Console.WriteLine("hello je suis là sinon le programme fonctionne pas");

var factory = new BrickContextFabric();

using var context = factory.CreateDbContext(); //le using sert à fermer correctement ce qui se trouve derrière le = lorque le programme se
                                               //ferme

//await AddData();
//await QueryData();
await QueryData2();

//récupérer les briques, leur disponibilité et les prix chez les différents vendeurs
async Task QueryData()
{
    var availabilityData = await context.BrickAvailabilities
        .Include(ba => ba.Brick)
        .Include(ba => ba.Vendor)
        .ToArrayAsync();

    foreach(var item in availabilityData)
    {
        Console.WriteLine($"Brick {item.Brick.Title} available at {item.Vendor.VendorName} for {item.Price}");
    }
}

//récupérer liste des briques avec les vendeurs de cette brique et les tags de cette brique
async Task QueryData2()
{
    var availabilityData = await context.Bricks
        .Include(ab => ab.Tags)
        //manuel
        //.Include("Availability.Vendor") //manual string navigation property path -> facile de faire une faute de frappe
        
        //semi assisté, mieux mais ça pique un peu
        //.Include($"{nameof(Brick.Availabilities)}.{nameof(BrickAvailability.Vendor)}")
        
        //méthode d'extension, meilleur et lisible
        .Include(b => b.Availabilities)
        .ThenInclude(a => a.Vendor) //theninclude se répercute sur le résultat du include, pas sur le résultat total (???)
        .ToArrayAsync();

    /*pour inclure le vendeurs àpd de BA
     * string path
     *      à la main
     *      semi assisté
     *      méthode d'extension
     */

    foreach (var item in availabilityData)
    {
        Console.WriteLine($"Bricks {item.Title}");
        Console.WriteLine($"{String.Join(',', item.Tags.Select(t => t.Title))}");
        Console.WriteLine($"is available at {String.Join(',', item.Availabilities.Select(b => b.Vendor.VendorName))}");
    }
}


async Task AddData()
{
    //on crée des pointeurs pour que ça soit moins chiant après pour les référencer
    Vendor brickLink, hotBrickes;
    Tag rare, ninjago, minecraft;

    await context.AddRangeAsync(new[]
    {
        brickLink = new Vendor() {VendorName = "Brick Link"},
        hotBrickes = new Vendor() {VendorName = "Hot Bricks"},
    });

    await context.SaveChangesAsync();

    await context.AddRangeAsync(new[]
    {
        rare = new Tag() {Title = "Rare"},
        ninjago = new Tag() {Title = "Ninjago"},
        minecraft = new Tag() {Title = "Minecraft"},
    });
    await context.SaveChangesAsync();

    await context.AddAsync(new BasePlate 
    {
        Title = "BasePlate 16*16 with blue water pattern",
        Color = Color.Blue,
        Tags = new() { rare, minecraft},
        Length = 16,
        Width = 16,
        Availabilities = new()
        {
            new() { Vendor = brickLink, Quantity = 5, Price = 6.5m },   //m spécifie qu'on travaille avec les décimales
            new() { Vendor = hotBrickes, Quantity = 10, Price = 5.9m }
            //si on met brickLink ici, EF fait le lien automatiquement et le brickId est automatiquement recherché
        }
    });

    await context.SaveChangesAsync();
}

//traduit en int dans la db
enum Color
{
    Red,
    Green,
    Blue,
    White,
    Orange
}

//relation entre Brick et Tag: many to many
//brick et brickavailability : one to many
//brickavailability et vendor : one to many

//on a une sule table brick qui regroupe les valeurs des classes filles, en plus d'un champ discriminator qui indique où se trouve la colonne
//à l'origine