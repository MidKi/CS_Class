//Language Integrated Query (LINQ) => collection

// 2 manières d'utilisation
//- Expressions de erquêtes ( Query syntax )
//- Méthodes d'extension ( Method syntax )

int[] numbers = { 4, 9, 5, 8, 65, 1, 84, 15 };

//Query syntax : 
IEnumerable<int> query1 =
    from num in numbers
    where num % 2 == 0
    orderby num
    select num;

//Console.WriteLine(String.Join(", ", query1));

//Method syntax
//méthode d'extension: rajouter une ou des fonctionnalités à une classe sans directement y toucher
IEnumerable<int> query2 = numbers.Where(num => num % 2 == 0).OrderBy(n => n);
//Console.WriteLine(String.Join(", ", query2));

//sources manipulables avec LINQ =>
// - Collections d'objets fortement typés: LINQ to Objects
// - Fichiers XML : LINQ to XML
// - ADO.NET : LINQ to Dataset
// - Entity Framework : LINQ to Entities

//IEnumerable => IEnumerator => MoveNext

var res = GenerateNumber(10).Where(val => val > 5);

//système pull-base = pattern observer
res = GenerateNumber(10)    //système pull-based (tire les infos quand il a besoin au lieu de tout prendre d'un coup)
    .Where(n =>
    {
        return n > 5;
    })
    .Select(n =>
    {
        return n * 3;
    });

/* idem que l'autre méthode, composition de requête/exécution diférrée
res = res.Where(n => n > 5);
res = res.Select(n => n * 3);*/

foreach (var item in res)
{
    Console.WriteLine(item);
}

IEnumerable<int> GenerateNumber(int maxValue)
{
    var result = new List<int>();

    for (int i = 0; i < maxValue; i++)
    {
        yield return i; //le yield change l'ordre d'exécution du code puique tout se fait un par un au lieu de faire chaque boucle à la fois
    }                   //permet de ne pas devoir stocker toutes les valeurs d'un coup, les valeurs sont traitées et supprimées progressivement
}