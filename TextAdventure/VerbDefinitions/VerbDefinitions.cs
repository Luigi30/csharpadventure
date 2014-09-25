using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Verbs
{
    class Verb : IVerb
    {
        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<String> Synonyms { get; set; }

        public Verb(String name){
            Name = name;
            Synonyms = new List<String>();
            Synonyms.Add(Name);
        }

        public Verb(String name, List<String> synonyms){
            Name = name;
            Synonyms = new List<String>();
            Synonyms.Add(Name);
            Synonyms.Concat(synonyms);
        }
    }
}
