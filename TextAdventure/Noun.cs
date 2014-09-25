using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class Noun : INoun
    {
        #region properties
        private String _name;
        private String _description;
        private Room _location;
        private String _definitearticle;
        private String _indefinitearticle;
        private String _singularname;

        private bool _isOpen;
        private bool _isScenery; //appears on a separate line when Looking at a room?

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public String DefiniteName
        {
            get { return String.Format("{0} {1}", DefiniteArticle.ToLower(), Name.ToLower()); }
        }

        public String IndefiniteName
        {
            get { return String.Format("{0} {1}", IndefiniteArticle.ToLower(), Name.ToLower()); }
        }

        public String SingularName
        {
            get { return _singularname; }
            set { _singularname = value; }
        }

        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public Room Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public String DefiniteArticle
        {
            get { return _definitearticle; }
            set { _definitearticle = value; }
        }

        public String IndefiniteArticle
        {
            get { return _indefinitearticle; }
            set { _indefinitearticle = value; }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
            set { _isOpen = value; }
        }

        public bool IsScenery
        {
            get { return _isScenery; }
            set { _isScenery = value; }
        }

        public List<Noun> Has { get; set; }
        #endregion

        public Noun()
        {
            DefiniteArticle = "the";
            IndefiniteArticle = "a";
            IsScenery = false;
        }

        public String Perform(IVerb verb, INoun actor, String verb_preposition = null)
        {
            //if a method Verb exists on the derived noun class, call it.
            MethodInfo method = this.GetType().GetMethod(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(verb.Name.ToLower()), new [] {typeof(INoun), typeof(String)});
            Object[] parameters = new Object[] { actor, verb_preposition };
            if (method != null)
            {
                return (string)method.Invoke(this, parameters);
            }
            else
            {
                return String.Format("Action not defined: Noun {0}, Verb {1}\r\n", Name, verb.Name);
            }
        }

        public String Perform(IVerb verb, INoun actor, INoun indirect_object, String verb_preposition = null, String indirect_object_preposition = null)
        {
            //if a method Verb exists on the derived noun class, call it.
            MethodInfo method = this.GetType().GetMethod(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(verb.Name.ToLower()));
            Object[] parameters = new Object[] { actor, indirect_object, verb_preposition, indirect_object_preposition };
            if (method != null)
            {
                return (string)method.Invoke(this, parameters);
            }
            else
            {
                return String.Format("Action not defined: Noun {0}, Verb {1}, Indirect Object {2}\r\n", Name, verb.Name, indirect_object.Name);
            }
        }

        #region Default verb implementations

        public String Take(INoun actor, String verb_preposition = null)
        {
            if (Name == null)
            {
                return String.Format("You can't take nothing!\r\n");
            }
            else
            {
                return String.Format("You can't take {0}.\r\n", DefiniteName);
            }
        }

        public String Put(Noun actor, String verb_preposition = null)
        {
            return String.Format("Put {0} in what?\r\n", DefiniteName);
        }

        public String Put(Noun actor, Noun indirect_object, String verb_preposition = null, String indirect_object_preposition = null)
        {

            //this object is going to be put inside another object.
            if (indirect_object.Has == null)
            {
                return String.Format("That can't contain other objects.\r\n", indirect_object.DefiniteArticle, indirect_object.Name);
            }
            else
            {
                //does the player have this item?
                if (GameManager.Instance.PC.Has.Find(x => x.Name == Name) == null)
                {
                    return String.Format("You don't have {0}.\r\n", DefiniteName);
                }

                var direct_object = GameManager.Instance.PC.Has.Find(x => x.Name == Name);

                GameManager.Instance.WorldItems.Find(x => x.Name == Name).Location = null;
                GameManager.Instance.WorldItems.Find(x => x.Name == indirect_object.Name).Has.Add(this);
                GameManager.Instance.PC.Has.Remove(direct_object);
                return String.Format("Okay, {0} is now inside {1}.\r\n", DefiniteName, indirect_object.DefiniteName);
            }
        }

        public String Drop(INoun actor, String verb_preposition = null)
        {
            if (actor == null)
            {
                return "Drop what?\r\n";
            }

            var item = GameManager.Instance.PC.Has.Find(x => x.Name == Name);
            if (item == null)
            {
                return String.Format("You don't have {0}.\r\n", DefiniteName);
            }
            else
            {
                GameManager.Instance.PC.Has.Remove(item);
                GameManager.Instance.WorldItems.Find(x => x.Name == Name).Location = GameManager.Instance.PC.Location;
                return String.Format("Dropped.\r\n");
            }
        }

        public String Look(INoun actor, String verb_preposition = null)
        {
            var singleton_item = GetWorldItem(Name);
            var output = "";

            if (verb_preposition == null || verb_preposition == "AT")
            {
                output += String.Format("{0}\r\n", Description);

            }
            else if (verb_preposition == "IN")
            {
                int i = 0;

                if (singleton_item.Has != null && singleton_item.Has.Count > 0)
                {
                    output += String.Format("Inside {0}", DefiniteName);
                    if (singleton_item.Has.Count > 1)
                    {
                        output += " are ";
                        foreach (var item in singleton_item.Has)
                        {
                            output += item.SingularName + ", ";
                            if (i == Has.Count)
                            {
                                output += "and ";
                            }
                            i++;
                        }
                        output += ".\r\n";
                    }
                    else
                    {
                        output += String.Format(" is {0}.\r\n", singleton_item.Has[0].DefiniteName);
                    }
                }
                else
                {
                    output += String.Format("Inside {0} is nothing.\r\n", DefiniteName);
                }
            }

            return output;

        }

        public String Open(INoun actor, String verb_preposition = null)
        {
            return "That can't be opened.\r\n";
        }

        public String Close(INoun actor, String verb_preposition = null)
        {
            return "That can't be closed.\r\n";
        }

        public INoun GetWorldItem(String name){
            return GameManager.Instance.WorldItems.Find(x => x.Name == Name);
        }

        #endregion
    }
}
