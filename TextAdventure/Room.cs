using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventure.Items;

namespace TextAdventure
{

    class Room : Noun
    {
        //private static int _id = 0;

        public struct Direction
        {
            public const string East = "EAST";
            public const string North = "NORTH";
            public const string South = "SOUTH";
            public const string West = "WEST";
        }

        public struct OppositeDirection
        {
            public const string East = "WEST";
            public const string North = "SOUTH";
            public const string South = "NORTH";
            public const string West = "EAST";
        }

        public int Id { get; set; }

        public Dictionary<string, int> Exits; //a dictionary containing a list of exits from this room and their cardinal directions.
        public Dictionary<string, Door> Doors;

        public Room(int id, String name, String description, Dictionary<String, int> exits)
        {
            //this.Id = System.Threading.Interlocked.Increment(ref _id);
            Id = id;
            Name = name;
            Description = description;
            Exits = exits;
            DefiniteArticle = "the";
            Location = this; //a Room's location is a pointer to itself
            Doors = new Dictionary<string, Door>();
        }

        public void AddReciprocalExit(String direction, Room target_room)
        {
            var rooms = GameManager.Instance.Rooms;

            var this_room = rooms.Find(x => x.Name == Name);

            this_room.Exits[direction] = target_room.Id;

            switch(direction){
                case Direction.East:
                    target_room.Exits[Direction.West] = Id;
                    break;
                case Direction.North:
                    target_room.Exits[Direction.South] = Id;
                    break;
                case Direction.South:
                    target_room.Exits[Direction.North] = Id;
                    break;
                case Direction.West:
                    target_room.Exits[Direction.East] = Id;
                    break;
            }
        }

        public void AddDoorToExit(String direction, INoun door)
        {
            var rooms = GameManager.Instance.Rooms;
            var this_room = rooms.Find(x => x.Name == Name);

            this_room.Doors[direction] = (Door)door;
            Console.WriteLine("Added door {0} to room {1} in direction {2}", door.Name, Name, direction);
        }

        public bool ExitHasOpenDoor(String direction)
        {
            return Doors[direction].IsOpen;
        }

        public String Look(INoun actor)
        {
            String output = "";

            output += String.Format("{0}\r\n{1}\r\n", Name, Description);

            var stuff = GameManager.Instance.WorldItems.FindAll(x => x.Location == GameManager.Instance.PC.Location).FindAll(x => x.IsScenery == false);
            var number_of_items = stuff.Count;

            if (number_of_items > 0)
            {
                output += "\tYou see ";

                for (int i = 0; i < number_of_items; i++)
                {
                    output += stuff[i].SingularName + ", ";
                    if (i == number_of_items - 2 && number_of_items > 1)
                    {
                        output += "and ";
                    }
                }

                char[] chars = {',', ' '};

                output = output.TrimEnd(chars);
                output += " here.\r\n";
            }

            return output;
        }
    }
}
