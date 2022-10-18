using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyBrick;
using System.ComponentModel.DataAnnotations;

Console.WriteLine("hello je suis là sinon le programme fonctionner pas");

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