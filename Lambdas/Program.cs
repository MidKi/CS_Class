/*class Hero1
{
    public Hero1(string firstName, string lastName, string heroName, bool canFly)
    {
        //...
    }
}*/

using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;

var heroes = new List<Hero>
{
    new("Bruce", "Wayne", "Batman", false),             //pas besoin de mettre "Hero" après si on sait que l'objet est Hero, peut toujours être
    new("Tony", "Stark", "Iron Man", true),             //utile pour l'héritage ou le polymorphisme, ou pour la lisibilité
    new(string.Empty, string.Empty, "Thanos", false),
    new(string.Empty, string.Empty, "Vision", true),
};

var resultFly = HeroesThatCanFly(heroes);
var heroesThatCanFly = string.Join(", ", resultFly);

var resultLastName = HeroesWithoutName(heroes);
var heroesWithoutName = string.Join(", ", resultLastName);

//test par délégué
//héro qui volent
var resultFilterFly = HeroesFilter(heroes, hero => hero.canFly);
var FilterFly = string.Join(", ", resultFilterFly);

//héros qui n'ont pas de nom
var resultFilterNoName = HeroesFilter(heroes, hero => string.IsNullOrEmpty(hero.lastName));
var FilterNoName = string.Join(", ", resultFilterNoName);

//les deux
var resultFilterNoNameCanFly = HeroesFilter(heroes, hero =>
{
    return string.IsNullOrEmpty(hero.lastName) && hero.canFly;
});
var FilterNoNameCanFly = string.Join(", ", resultFilterNoNameCanFly);

//test generic
var resultGeneric = HeroesFilter(new[] { 1, 2, 3, 4, 5 }, f => f % 2 == 0);
var GenericFilter = string.Join(", ", resultGeneric);

//test generic with func
var resultGenericFunc = HeroesFilter2(new[] { "Bob", "Harry", "John", "Carter", "Barry" }, f => f.StartsWith("B"));
var GenericFunc = string.Join(", ", resultGenericFunc);

//
var resultGenericFunc2 = HeroesFilter2(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, x => x % 2 != 0);
/*
//compte à l'infini
var watch = Stopwatch.StartNew();
CountToInfinity();
watch.Stop();

//compte à l'infini 2
Benchmark(CountToInfinity); //pas de parenthèses pour CountToinfinity parce qu'on veut pas l'appeler

//Action
Benchmark2(CountToInfinity);*/

//pas de paramètres, parenthèses vide
/*Action a1 = () => Console.WriteLine("hi");
a1();*/

//un seul paramètre, parenthèses pas obligatoires
/*Action<int> a2 = n => Console.WriteLine(n*n); //n = fonction
a2(8);*/

//plusieurs paramètres, parenthèses obligatoires
/*Action<string, string> a3 = (s1, s2) => Console.WriteLine(s1+s2);
a3("hello", "world");*/

/*Func<int> f1 = () => 24;
Console.WriteLine(f1());*/

/*Func<int, int> f2 = n => n * n;
Console.WriteLine(f2(9));*/

/*Func<int, int, bool> f3 = (x, y) => x == y;
Console.WriteLine(f3(12, 12));*/

Calc3(5);

Func<int, int> calculator = CreateCalculator();
Console.WriteLine(calculator(20));

//no param -> .. = () => ..
//one param -> .. = param => .. OR .. = (param) => ..
//more than one param -> .. = (param1, param2) => ..

//
int Calc2(int n)
{
    var factor = 8;     //facteru est hors de portée
    return factor * n;
}

int Calc3(int n)
{
    if (n == 0) return 0;
    var factor = 8;
    return factor * Calc3(n - 1);
}

//fonction d'ordre supérieur: reçoit et/ou retourne une fonction
Func<int, int> CreateCalculator()
{
    var factor = 8;
    return n => n * factor;     //la méthode est exécutée avant d'appeler calculatrice, donc facteur devrait être hors de portée au moment
}                               //du return (voir Calc2()) (le code est executé mais pas la fonction se trouvant dans le return)
//closure permet d'étendre la portée d'une variable au-delà de son scope initial

Demo CreateCalculatorInternal()
{
    return new Demo { factor = 0 };
}

//filtre héros qui peuvent voler
List<Hero> HeroesThatCanFly(List<Hero> heroes)
{
    var result = new List<Hero>();

    foreach (var hero in heroes)
    {
        if (hero.canFly)
        {
            result.Add(hero);
        }
    }

    return result;
}

//filtre héros sans nom
List<Hero> HeroesWithoutName(List<Hero> heroes)
{
    var result = new List<Hero>();

    foreach (var hero in heroes)
    {
        if (string.IsNullOrEmpty(hero.lastName))
        {
            result.Add(hero);
        }
    }

    return result;
}

//filtre item, fonctionne pour tout type d'item
IEnumerable<T> HeroesFilter<T>(IEnumerable<T> items, FilterGeneric<T> filter) //on peut utiliser n'importe quelle structure qui implémente IEnumerable
{
    foreach (var item in items)
    {
        if (filter(item))               //si l'item répond au test du filtre
        {
            //yield permet de créer l'élément hero à ajouter dans le IEnumerable
            yield return item;          //a recaster en une autre structure pour pas perdre de fonctionnalités
        }
    }
}

IEnumerable<T> HeroesFilter2<T>(IEnumerable<T> items, Func<T, bool> filter) //func<> idem que créer un predicate manuellement
{
    foreach (var item in items)
    {
        if (filter(item))
        {
            if (filter(item))
                yield return item;    //le if pas obligé maybe?
        }
    }
}

void CountToInfinity()
{
    for (int i = 0; i < int.MaxValue; i++) ;
}

void Benchmark(Count c)
{
    var watch2 = Stopwatch.StartNew();
    c();
    watch2.Stop();
    Console.WriteLine("watch2: " + watch2.Elapsed);
}

void Benchmark2(Action a)   //pas de Func car il n'y a pas de type de retour, on replace par Action
{
    var watch3 = Stopwatch.StartNew();
    a();
    watch3.Stop();
    Console.WriteLine("watch3: " + watch3.Elapsed);
}

int Benchmark3(Func<int> f)
{
    var watch3 = Stopwatch.StartNew();
    var result = f();
    watch3.Stop();
    Console.WriteLine("watch3: " + watch3.Elapsed);

    return result;
}

int Calc()
{
    for (int i = 0; i < int.MaxValue; i++) ;
    return 69;
}

//prints
/*Console.WriteLine("resultFly: " + heroesThatCanFly);
Console.WriteLine("resultNoName: " + heroesWithoutName);
Console.WriteLine("resultFilterFly: " + FilterFly);
Console.WriteLine("resultFilerNoName: " + FilterNoName);
Console.WriteLine("resultFilterNoNameCanFly: " + FilterNoNameCanFly);
Console.WriteLine("resultGeneric: " + GenericFilter);
Console.WriteLine("resultGenericFunc: " + GenericFunc);
Console.WriteLine("resultGenericFunc2: " + string.Join(", ", resultGenericFunc2));
Console.WriteLine("result watch: " + watch.Elapsed);
Console.WriteLine($"Calc(): { Benchmark3( () => Calc()) }");*/

//func = delegate retour T
//actoin = delegate retour void
//predicate = delegate retour bool

delegate void Count();

//predicate
delegate bool Filter(Hero hero); //renvoie un booleen vrai ou faux par rapport au filtre, c'es le if qu'on veut rendre universel
delegate bool FilterGeneric<T>(T item); //aussi un predicate

record Hero(string firstName, string lastName, string heroName, bool canFly); //idem que créer un constructeur normal



class Demo
{
    public int factor;
    public int Calculator(int n) => n * factor;
}
