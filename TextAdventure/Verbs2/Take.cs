using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Verbs
{
    class Take : IVerb
    {
        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Take()
        {
            Name = "TAKE";
        }

        public String Perform()
        {
            return "Take what?";
        }

        public String Perform(INoun noun)
        {
            noun.Location = GameMgr.Instance.Rooms.Find(x => x.Name == "Player's Inventory");
            return "Taken.\r\n";
        }
    }
}
