using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;
using TextAdventure.Verbs;

namespace TextAdventure
{
    sealed class GameManager
    {
        static readonly GameManager _instance = new GameManager();
        public static GameManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<Room> Rooms;
        public List<INoun> WorldItems;

        public SentenceParser Parser;
        public List<IVerb> ValidVerbs;

        public Player PC;

        public GameManager()
        {
            Rooms = InitRooms();
            ValidVerbs = InitVerbs();
            WorldItems = InitNouns();

            Parser = new SentenceParser(VerbDictionary, WorldItems);

            PC = new Player(Rooms[0]);
        }

        Dictionary<String, IVerb> VerbDictionary = new Dictionary<String, IVerb>();

        public String StartGame()
        {
            CreateRoomExits();

            return PC.Perform(VerbDictionary["LOOK"], PC);
        }

        public String Process(String input)
        {
            if (!Parser.Parse(input))
            {
                return Parser.ParseError;
            }
            else
            {
                var result = Parser.Result;

                if (result.ParsedNoun == null)
                {
                    //verbs that act on the player
                    return PC.Perform(result.ParsedVerb, PC, null);
                }
                else if (result.ParsedIndirectObject == null)
                {
                    return WorldItems.Find(x => x.Name == result.ParsedNoun.Name).Perform(result.ParsedVerb, PC, result.ParsedVerbPreposition);
                }
                else
                {
                    return WorldItems.Find(x => x.Name == result.ParsedNoun.Name).Perform(result.ParsedVerb, PC, result.ParsedIndirectObject, result.ParsedVerbPreposition, result.ParsedIndirectObjectPreposition);
                }
            }
        }

        #region world initialization
        private List<IVerb> InitVerbs()
        {
            var verbs = new List<IVerb>();

            verbs.Add(new Verb("LOOK"));
            verbs.Add(new Verb("TAKE"));
            verbs.Add(new Verb("INVENTORY"));
            verbs.Add(new Verb("PUT"));
            verbs.Add(new Verb("DROP"));
            verbs.Add(new Verb("EAST"));
            verbs.Add(new Verb("NORTH"));
            verbs.Add(new Verb("SOUTH"));
            verbs.Add(new Verb("WEST"));
            verbs.Add(new Verb("OPEN"));
            verbs.Add(new Verb("CLOSE"));

            foreach (var verb in verbs)
            {
                foreach (var synonym in verb.Synonyms)
                {
                    VerbDictionary.Add(synonym, verb);
                }
            }

            return verbs;
        }
        private List<INoun> InitNouns()
        {
            var nouns = new List<INoun>();
            nouns.Add(new Items.Sword(Rooms.Find(x => x.Name == "Test Room")));
            nouns.Add(new Items.Debit_Card(Rooms.Find(x => x.Name == "Test Room")));
            nouns.Add(new Items.Butt(Rooms.Find(x => x.Name == "Test Room")));
            nouns.Add(new Items.Door(Rooms.Find(x => x.Name == "Test Room")));
            return nouns;
        }
        private List<Room> InitRooms()
        {
            var rooms = new List<Room>();

            rooms.Add(new Room(0, "Test Room",
                "This is a testing room.",
                new Dictionary<String, int>()));

            rooms.Add(new Room(1, "Test Room East",
                "This is a testing room east of the other testing room.",
                new Dictionary<String, int>()));

            rooms.Add(new Room(2, "Indefinite Location",
                "This is a placeholder location.",
                new Dictionary<String, int>()));

            return rooms;
        }
        private void CreateRoomExits()
        {
            //room exits
            Rooms.Find(x => x.Name == "Test Room").AddReciprocalExit(Room.Direction.East, Rooms.Find(x => x.Name == "Test Room East"));
            Rooms.Find(x => x.Name == "Test Room").AddDoorToExit(Room.Direction.East, WorldItems.Find(x => x.Name == "DOOR"));
        }
        #endregion
    }
}
