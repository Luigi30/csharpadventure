using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Verbs
{
    class Look : IVerb
    {
        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Look()
        {
            Name = "LOOK";
        }

        public String Perform() //LOOK with no noun looks at the player's location.
        {
            String output = "";

            var noun = GameMgr.Instance.PC.Location;

            output += noun.Name + "\r\n" + noun.Description + "\r\n";

            var stuff = GameMgr.Instance.WorldItems.FindAll(x => x.Location == GameMgr.Instance.PC.Location);

            if (stuff.Count > 0)
            {
                output += "You see ";
                foreach (var item in stuff)
                {
                    output += item.Name;
                }
                output += " here.\r\n";
            }

            return output;
        }

        public String Perform(INoun noun) //LOOK with a noun looks at the noun.
        {
            String output = "";

            output += noun.Description + "\r\n";
            return output;
        }
    }
}