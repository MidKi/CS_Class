using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyBrick;
using System.ComponentModel.DataAnnotations;

Console.WriteLine("hello je suis là sinon le programme fonctionner pas");

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