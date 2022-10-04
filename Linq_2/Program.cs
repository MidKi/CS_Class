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

// voitures les plus puissantes
//.Skip à la place ou avant .Take pour ignorer les premières occurences (nb au choix) (essentiellement de la pagination), on peut aussi faire .take avant .skip mais il faut faire attention
//voitures.OrderByDescending(v => v.Puissance).Take(10).Select(v => $"{v.Maker} { v.Modele} | {v.Puissance}").ToList().ForEach(v => Console.WriteLine(v));

//afficher le nb de modèles par marque construit après 1995
voitures.Where(v => v.Annee > 1995).GroupBy(v => v.Maker).Select(v => v.Count()).ToList().ForEach(v => Console.WriteLine(v));
Console.WriteLine("---------------------------------Fin version perso---------------------------------");
//version prof
voitures.GroupBy(v => v.Maker).Select(v => new { v.Key, NombreModeles = v.Count() }).ToList()
    .ForEach(i => Console.WriteLine($"{i.Key} : {i.NombreModeles}"));

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