using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBrick
{
    class BrickAvailability //on ne doit que rajouter les classes d'association si on veut rajouter des attributs
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(8, 2)")] //8 digits dont 2 après virgule max, permet de garder la valeur exacte et éviter d'arondir
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Vendor? Vendor { get; set; } //propriété de navigation, sert à accéder à VendroId côté C#

        public int VendorId { get; set; } //clé étrangère pour la db

        public Brick Brick { get; set; } //prop de navigatino pour C#
        public int BrickId { get; set; } //clé étrangère pour la db
    }
}
