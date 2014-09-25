using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    interface IVerb
    {
        //What is this verb's keyword?
        String Name { get; set; }
        List<String> Synonyms { get; set; }
    }
}
