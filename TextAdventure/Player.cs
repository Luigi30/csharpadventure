using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class Player : Noun
    {
        public Player(Room location = null)
        {
            Location = location;
            Contents = new List<Noun>();
        }

        public new String Look(INoun actor, String verb_preposition = null)
        {
            //look at the current location.
            return Location.Look(actor);
        }

        public String Inventory(INoun actor, String verb_preposition = null)
        {
            string output = "You are carrying:\r\n";

            if (Contents.Count == 0)
            {
                output += "\tnothing";
            }
            else
            {
                foreach (var item in Contents)
                {
                    output += String.Format("\t{0}\r\n", item.IndefiniteName);
                }
            }

            return output + "\r\n";
        }

        public String Go(String direction)
        {
            if (Location.Exits.ContainsKey(direction))
            {
                if (!Location.Doors.ContainsKey(direction) || Location.ExitHasOpenDoor(direction))
                {
                    var rooms = GameManager.Instance.Rooms;
                    var other_room = rooms.Find(x => x.Id == Location.Exits[direction]);

                    Location = other_room;
                    return Look(this);
                }
                else
                {
                    return "The door in that direction is closed.\r\n";
                }

            }
            else
            {
                return "There is no exit in that direction.\r\n";
            }
        }

        public String East(INoun actor, String verb_preposition = null)
        {
            return Go(Room.Direction.East);
        }

        public String West(INoun actor, String verb_preposition = null)
        {
            return Go(Room.Direction.West);
        }

        public String North(INoun actor, String verb_preposition = null)
        {
            return Go(Room.Direction.North);
        }

        public String South(INoun actor, String verb_preposition = null)
        {
            return Go(Room.Direction.South);
        }
    }
}
