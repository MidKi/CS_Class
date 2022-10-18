using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBrick
{
    class Tag
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Title { get; set; } = String.Empty;

        //un tag peut être associé à 0 ou plusieurs briques
        public List<Brick> Bricks { get; set; } = new();
    }
}
