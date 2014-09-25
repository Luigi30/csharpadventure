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
            Location = null;
            taker.Contents.Add(this);
            return "Taken.\r\n";
        }

    }
}
