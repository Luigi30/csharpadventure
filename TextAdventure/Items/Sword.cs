using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Items
{
    class Sword : Noun
    {
        public Sword(Room location)
        {
            Name = "SWORD";
            Description = "A sword. The blade is chipped and the handle's leather wrapping is tarnished.";
            Location = location;
            SingularName = IndefiniteName;
        }

        public new String Take(INoun actor, String verb_preposition)
        {
            if (Location == actor.Location)
            {
                Location = null;
                actor.Contents.Add(this);
                return "Taken.\r\n";
            } else {
                return String.Format("There's no {0} here.\r\n", Name.ToLower());
            }

        }
    }
}
