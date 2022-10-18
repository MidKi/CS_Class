using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBrick
{
    class Vendor
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string VendorName { get; set; } = String.Empty;

        public List<BrickAvailability> BrickAvailabilities { get; set; } //le vendeur a une référence des briques
    }
}
