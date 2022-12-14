using System.Text.Json;
using System.Text.Json.Serialization;

var fileContent = await File.ReadAllTextAsync("MOCK_DATA.json");
var voitures = JsonSerializer.Deserialize<Voiture[]>(fileContent);

var voitureAvecQuatrePortesOuPlus = voitures.Where(v => v.Portes >= 4);

/*
foreach(var voiture in voitureAvecQuatrePortesOuPlus)
{
    Console.WriteLine($"La voiture {voiture.Maker} {voiture.Modele} possède {voiture.Portes} portes");
}*/

//si on fait v => v.Constructeur, il n'y aura que ça à afficher
//var marquePlusModelesAvecM = voitures.Where(v => v.Maker.StartsWith("M")).Select(v => $"Marque: {v.Maker} | Modèle: {v.Modele}").ToList();

//pas de foreach ici si assignation à une variable puisque foreach ne retourne rien
//voitures.Where(v => v.Maker.StartsWith("M")).Select(v => $"Marque: {v.Maker} | Modèle: {v.Modele}").ToList().ForEach(v => Console.WriteLine(v)); 

/*foreach (var voiture in marquePlusModelesAvecM)
{
    //Console.WriteLine($"Marque: {voiture.Maker} | Modèle: {voiture.Modele}");   //voiture est devenu une chaine de caractères, donc plus moyen de faire ça
    Console.WriteLine($"{voiture}");
}*/

/*
// voitures les plus puissantes
//.Skip à la place ou avant .Take pour ignorer les premières occurences (nb au choix) (essentiellement de la pagination), on peut aussi faire .take avant .skip mais il faut faire attention
voitures.OrderByDescending(v => v.Puissance).Take(10).Select(v => $"{v.Maker} { v.Modele} | {v.Puissance}").ToList().ForEach(v => Console.WriteLine(v));
*/


//afficher le nb de modèles par marque construit après 1995
/*
voitures.Where(v => v.Annee > 1995).GroupBy(v => v.Maker).Select(v => v.Count()).ToList().ForEach(v => Console.WriteLine(v));
Console.WriteLine("---------------------------------Fin version perso---------------------------------");
//version prof
voitures//.Where(v => v.Annee > 1995)
        .GroupBy(v => v.Maker)
        //.Select(v => new { v.Key, NombreModeles = v.Where(v => v.Annee > 1995).Count() })
        .Select(v => new { v.Key, NombreModeles = v.Count(v => v.Annee > 1995), carAttributes = v.Select(v => v) })
        .ToList()
        .ForEach(i => 
        {   //varAttributes possède toutes les infos de la voiture, on utilise le .select pour récupérer une information précise
            //le string.join nécessite une projection pour le modèle de voiture
            Console.WriteLine($"{i.Key} : {i.NombreModeles} {string.Join(" - ", i.carAttributes.Select(v => v.Modele) )}");
        });
*/

//afficher les constructeus qui ont au moins 2 modèles de puissance >= 400 et la quantité de modèles par constructeur
/*
voitures.Where(v => v.Puissance >= 400)
        .GroupBy(v => v.Maker)
        .Select(v => new { Constructeur = v.Key, NbVoitures = v.Count() }) //pas besoin de mettre un prédicat dans le count pcq on a 
        .Where(constr => constr.NbVoitures >= 2)                           //déjà supprimé les voitures avec <400 hp
        .ToList()
        .ForEach(constr => Console.WriteLine($"{constr.Constructeur} : {constr.NbVoitures}"));
*/

//afficher la puissance moyenne par constructeur
/*
voitures.GroupBy(v => v.Maker)
        .Select(v => new { Constructeur = v.Key, Puissance = v.Average(v => v.Puissance) })
        .ToList()
        .ForEach(v => Console.WriteLine($"{v.Constructeur} : {v.Puissance}"));
*/

//afficher le nombre de voitures par constructeur par tranche de puissance 0->100, 100->200, 200->300, ...
/*
voitures.GroupBy(v => v.Puissance switch
{
    <= 100 => "0-100",
    <= 200 => "101-200",
    <= 300 => "201-300",
    <= 400 => "301-400",
    _ => "401-...",
})
        .OrderBy(v => v.Key)
        .Select(v => new { HPCategory = v.Key, NbMakers = v.Select(v => v.Maker).Distinct().Count() })
        .ToList()
        .ForEach(v => Console.WriteLine($"{v.HPCategory} : {v.NbMakers}"));
*/


class Voiture
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("car_make")]
    public string Maker { get; set; }

    [JsonPropertyName("car_model")]
    public string Modele { get; set; }

    [JsonPropertyName("car_year")]
    public int Annee { get; set; }

    [JsonPropertyName("number_of_doors")]
    public int Portes { get; set; }

    [JsonPropertyName("hp")]
    public int Puissance { get; set; }
}