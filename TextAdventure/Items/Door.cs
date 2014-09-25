using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Items
{
    class Door : Noun
    {
        public Door(Room location)
        {
            Name = "DOOR";
            Description = "A door.";
            Location = location;
            SingularName = IndefiniteName;
            IsScenery = true;
        }

        public new String Open(INoun actor, String verb_preposition)
        {
            var global_copy = GameManager.Instance.WorldItems.Find(x => x.Name == Name);

            if (global_copy.IsOpen == true)
            {
                return "It's already open.\r\n";
            }
            else
            {
                global_copy.IsOpen = true;
                return "With a creak, the door opens.\r\n";
            }
        }

        public new String Close(INoun actor, String verb_preposition)
        {
            var global_copy = GameManager.Instance.WorldItems.Find(x => x.Name == Name);

            if (global_copy.IsOpen == false)
            {
                return "It's already closed.\r\n";
            }
            else
            {
                global_copy.IsOpen = false;
                return "With a creak, the door closes.\r\n";
            }
        }
    }
}
