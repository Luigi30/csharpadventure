using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Items
{
    class Butt : Noun
    {
        public Butt(Room location)
        {
            Name = "BUTT";
            Description = "A butt.";
            Location = location;
            Has = new List<Noun>();
            SingularName = DefiniteName;
        }

        public new String Take(INoun actor, String verb_preposition){
            return "It's firmly attached to the floor.\r\n";
        }

    }
}
