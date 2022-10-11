//EF: ORM (Object Relational Mapper) => lien entre objets et tables
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
//Entity Framework repose sur le pattern Unit of Work
//Change Tracking sui va indiquer au système qu'est-ce qui a été modifié, et les requêtes seront générées en fonction de ça


//Classe Plat
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.ObjectiveC;
using System.Xml.Xsl;

var factory = new RecettesContextFabric();
using var  context = factory.CreateDbContext(); // libération des ressources à la fermeture de l'app

Console.WriteLine("On ajoute des céréales au petit déjeuner");
var cereales = new Plat { Titre = "Petit déjeuner aux céréales", 
                          Notes = "Avec un peu de lait c'est encore meilleur",
                          Avis = 4                        
};


//ajout dans la db
context.Plats.Add(cereales);
await context.SaveChangesAsync();


context.Plats.Remove(cereales);
await context.SaveChangesAsync();


//changement dans la db, faisable car on enregistre le pointeur vers ces données
cereales.Avis = 5;
await context.SaveChangesAsync();

//vérification avis céréales
Console.WriteLine("Vérification avis céréales");
var plat = await context.Plats.Where(plat => plat.Titre.Contains("céréales")).ToListAsync();

if(plat.Count != 1)
{
    Console.WriteLine("pas de céréales");
}
else
{
    Console.WriteLine($"le plat de céréales a {plat[0].Avis} avis/étoiles");
}

//pates

var pates = new Plat
{
    Titre = "Petit déjeuner aux pâtes mais sans sauce",
    Notes = "Avec un peu de sauce c'est encore meilleur!",
    Avis = 1
};

context.Plats.Add(pates);

Console.WriteLine($"Plat de pate {pates.Id} pas encore ajouté");
await context.SaveChangesAsync();
Console.WriteLine($"Plat de pates {pates.Id} ajouté");



var nvPlat = new Plat
{
    Titre = "Pipo",
    Notes = "Inzhagi"
};
context.Plats.Add(nvPlat);
await context.SaveChangesAsync();

nvPlat.Notes = "Internet cassé";
await context.SaveChangesAsync();

await EntityStates(factory);
await ChangeTracking(factory);
await AttachEntities(factory);
await NoTracking(factory);
await RawSql(factory);

static async Task EntityStates(RecettesContextFabric factory)
{
    using var context = factory.CreateDbContext();              //createdbcontext contient les plats et la db (+-)
    var nvPlat = new Plat { Titre= "John", Notes="Wick" };
    var state = context.Entry(nvPlat).State;
    //détaché = objet en mémoire mais inconnue de la DB et du context
    context.Plats.Add(nvPlat);
    state = context.Entry(state).State;
    //ajouté = objet en mémoire mais inconnu en DB et connu dans le context
    await context.SaveChangesAsync();
    state = context.Entry(nvPlat).State;
    //inchangé = objet en mémoire idenitque à la DB
    state = context.Entry(state).State;
    //modifié = objet en mémoire diférent de celui en DB
    context.Plats.Remove(nvPlat);
    state = context.Entry(nvPlat).State;
    //supprimé = objet en mémoire supprimé par rapport à la DB
    //si on Add nvPlat on retouren à l'état détaché
}

static async Task ChangeTracking(RecettesContextFabric factory)
{
    var context = factory.CreateDbContext();
    var nvPlat = new Plat { Titre = "Elden", Notes = "Ring" };
    context.Plats.Add(nvPlat);
    await context.SaveChangesAsync();
    nvPlat.Notes = "Lord";

    var entry = context.Entry(nvPlat);

    //SingleAsync vérifie d'abord le context, s'il n'y a rien alors il va check dans la DB
    //On peut créer autant de context qu'on veut -> chaque context a son propre système de tracking (état, OriginalValue, ...)
    //En créer plusieurs peut vite consommer beaucoup de mémoire
    //Pour forcer le check dans al DB, soit on crée un nouveau context, soit on désactive le tracking/détache les données
    //code managé: code controlé par le manager (machine virtuelle)

    //Ici on interroge la mémoire, ici l'objet existe dans la mémoire
    var originalValue = entry.OriginalValues[nameof(Plat.Notes)].ToString();
    var platInDB = await context.Plats.SingleAsync(p => p.Id == nvPlat.Id);

    //Ici on interroge la DB car le context est vide, l'Id nvPlat ne se trouve pas dans context2 mais existe dans la DB
    using var context2 = factory.CreateDbContext();
    var platInDB2 = await context.Plats.SingleAsync(p => p.Id == nvPlat.Id);
}

static async Task AttachEntities(RecettesContextFabric factory)
{
    var context = factory.CreateDbContext();
    var nvPlat = new Plat { Titre = "Elden", Notes = "Ring" };

    context.Plats.Add(nvPlat);

    await context.SaveChangesAsync();

    //faire oublier l'objet à EF (entity framework), permet de frocer une interrogation à la DB
    //context.Entry(nvPlat).State = EntityState.Detached;

    var state = context.Entry(nvPlat).State;
    context.Plats.Update(nvPlat);
    state = context.Entry(nvPlat).State;
    await context.SaveChangesAsync();
}

static async Task NoTracking(RecettesContextFabric factory)
{
    using var context = factory.CreateDbContext();

    //Select * From Plats
    var Plats = await context.Plats.AsNoTracking().ToListAsync(); //pas de tracking = détaché
    var state = context.Entry(Plats[0]).State;
}

static async Task RawSql(RecettesContextFabric factory)
{
    using var context = factory.CreateDbContext();
    var plats = await context.Plats
                .FromSqlRaw("SELECT * FROM Plats")
                .ToArrayAsync();

    var filtre = "%e";
    plats = await context.Plats
            .FromSqlInterpolated($"SELECT * FROM Plats WHERE Notes LIKE {filtre}")
            .ToArrayAsync();
}



#region classes
class Plat
{
    public int Id { get; set; } //primary key par convention (.NET détecte tout seul que Id = clé primaire -> auto incrémentation)
    
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

        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(optionsBuilder => optionsBuilder.AddConsole()))//;
                      .UseSqlServer(config["ConnectionStrings:peepoConnection"]);   //peepoConnection peut être n'importe quoi tant que ça
                                                                                    //correspond au json
        return new RecettesContext(optionsBuilder.Options);
    }
}
#endregion classes