using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    interface INoun
    {

        Room Location { get; set; }
        String Name { get; set; }
        String Description { get; set; }
        String DefiniteArticle { get; set; }

        String DefiniteName { get; }
        String IndefiniteName { get; }

        String SingularName { get; set; }

        List<Noun> Contents { get; set; }

        bool IsScenery { get; set; }

        #region possible properties
        bool IsOpen { get; set; }
        #endregion


        //A verb method to be called on this noun by an actor. (TAKE SWORD.)
        String Perform(IVerb verb, INoun actor, String verb_preposition);

        //A verb method to be called on this noun by an actor with an indirect object. (PUT SWORD IN BUTT.)
        String Perform(IVerb verb, INoun actor, INoun indirect_object, String verb_preposition, String indirect_object_preposition);
    }
}
