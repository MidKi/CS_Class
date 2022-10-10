﻿//EF: ORM (Object Relational Mapper) => lien entre objets et tables
//EF supporte +rs Provider DB:
/*
 * MSSQL Server
 * SQLite
 * Postgre
 * MySQL
 * In Memory (pratique pour les tests)
 * ...
 */

// => package Microsoft.EntityFrameowrkCore
//pour installer le package, outils -> gestionnaire de package NuGet -> Gérer les packages NuGet pour la solution
//-> parcourir et rechercher Entity puis installer les packages EntityFrameowrk
// on va utiliser SQL Server Express (vérifier si c'est installé: sqllocaldb i dans un cmd

//Recettes de cuisine
/*Besoine de:
 * DB
 * Tables
 * Classe Plat, Ingredient
 */

//Plusieurs approches possibles (code first, db first et model first)
//DBContext: version type d'une connexion à noter DB, contient les points d'entrée pour interroger la DB,
//les tables, ajouter, supprimer, modifier des données, ...


#region classes
//Classe Plat
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


Console.WriteLine("hello");

class Plat
{
    public int IdPlat { get; set; } //primary key par convention (.NET détecte tout seul que Id = clé primaire -> auto incrémentation)
    
    [MaxLength(100)]
    public string Titre { get; set; } = String.Empty;   //vide par défaut, pas null mais obligatoire

    
    public string? Notes { get; set; }  //? = nullable, comme en Dart
    
    public int? Avis { get; set; }

    public List<Ingredient> Ingredients { get; set; } = new(); //force une liste vide
}

//class Ingredients
class Ingredient
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    public string UniteDeMesure { get; set; } = string.Empty;

    [Column(TypeName = "decimal(5,2)")] //(5, 2) : 5 chiffres max, 2 après la virgule au max
    public decimal Quantite { get; set; }

    //stranger key vers Plat, on part du principe qu'un ingrédient n'est utilisé que dans un plat
    public int PlatId { get; set; }

    //propriété de navigation, permet d'atteindre plat (par ex: ingerdient.plat.proprieteDePlat
    public Plat? Plat { get; set; }
}

class RecettesContext : DbContext
{
    public DbSet<Ingredient> Ingredients { get; set; }

    public DbSet<Plat> Plats { get; set; }

    public RecettesContext(DbContextOptions<RecettesContext> options) : base(options)
    {
        
    }
}

//Fabrique de classe, sera intégré au système dans la partie web et pas à écrire soi-même
class RecettesContextFabric : IDesignTimeDbContextFactory<RecettesContext>
{
    public RecettesContext CreateDbContext(string[]? args = null)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<RecettesContext>();

        optionsBuilder//.UseLoggerFactory(LoggerFactory.Create(optionsBuilder => optionsBuilder.AddConsole()));
                      .UseSqlServer(config["ConnectionStrings:peepoConnection"]);   //peepoConnection peut être n'importe quoi tant que ça
                                                                                    //correspond au json
        return new RecettesContext(optionsBuilder.Options);
    }
}
#endregion classes