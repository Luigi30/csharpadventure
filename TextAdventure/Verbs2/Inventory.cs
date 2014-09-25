using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Verbs
{
    class Inventory : IVerb
    {
        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Inventory()
        {
            Name = "INVENTORY";
        }

        public String Perform()
        {
            string output = "You are carrying: ";
            
            //get every item in the player's inventory
            var player_items = GameMgr.Instance.PC.Inventory;

            if (player_items.Count == 0)
            {
                output += "Nothing";
            }
            else
            {
                foreach (var item in player_items)
                {
                    output += String.Format("\r\n\t{0}\r\n", item.Name);
                }
            }

            return output + "\r\n";
        }

        public String Perform(INoun noun)
        {
            return String.Format("You can't see the inventory of {0} {1}!\r\n", noun.Article, noun.Name);
        }
    }
}
