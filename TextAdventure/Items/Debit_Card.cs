using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Items
{
    class Debit_Card : Noun
    {
        public Debit_Card(Room location)
        {
            Name = "DEBIT CARD";
            Description = "A blue plastic debit card. The account number and name are embossed on the front. A black magnetic strip and phone numbers for international banks are on the back.";
            Location = location;
            SingularName = IndefiniteName;
        }

        public new String Take(INoun taker, String verb_preposition)
        {
            GameManager.Instance.WorldItems.Find(x => x.Name == this.Name).Location = null;
            GameManager.Instance.PC.Has.Add(this);
            return "Taken.\r\n";
        }

    }
}
