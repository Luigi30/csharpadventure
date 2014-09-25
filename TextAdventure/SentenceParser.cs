using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TextAdventure
{
    class SentenceParser
    {
        /* A valid sentence is in the format:
         *  [PERSON,] [VERB PHRASE] [PREPOSITION] [DIRECT OBJECT PHRASE] [PREPOSITION] [INDIRECT OBJECT PHRASE]
         *  
         * player is implied person
        */

        public struct ParseData
        {
            public IVerb ParsedVerb;
            public String ParsedVerbPreposition;
            public INoun ParsedNoun;
            public String ParsedIndirectObjectPreposition;
            public INoun ParsedIndirectObject;
            public ParsedSentence Sentence;
        }

        public struct ParsedSentence
        {
            public String VerbPhrase;
            public String ParsedVerbPreposition;
            public String DirectObject;
            public String ParsedIndirectObjectPreposition;
            public String IndirectObject;
        }

        static private INoun previousNoun; //used for the pronoun "it"

        public ParseData Result;
        public String ParseError;

        private Dictionary<String, IVerb> verbDictionary;
        private List<INoun> validNouns;
        private List<String> prepositions;
        private String workingSentence;


        public SentenceParser(Dictionary<String, IVerb> _verbDictionary, List<INoun> _validNouns)
        {
            verbDictionary = _verbDictionary;
            validNouns = _validNouns;

            prepositions = new List<String>(new String[] { "IN", "AT", "TO", "FROM" });
        }

        public bool Parse(string _input)
        {
            Result = new ParseData();

            workingSentence = _input.ToUpper();

            if (previousNoun != null && Regex.IsMatch(workingSentence, " IT$", RegexOptions.IgnoreCase) == true)
            {
                workingSentence = workingSentence.Replace(" IT", String.Format(" {0}", previousNoun.Name));
            }
            else if (previousNoun != null && Regex.IsMatch(workingSentence, " IT ", RegexOptions.IgnoreCase) == true)
            {
                workingSentence = workingSentence.Replace(" IT ", String.Format(" {0} ", previousNoun.Name));
            }

            //find sentence phrases
            DoParse(workingSentence);

            if (Result.Sentence.VerbPhrase == null)
            {
                ParseError = "[I couldn't understand that sentence.]\r\n";
                return false;
            }

            //split sentence into verb and noun.
            string[] split_sentence = workingSentence.Split(' ');

            try
            {
                Result.ParsedVerb = verbDictionary[Result.Sentence.VerbPhrase];
            } catch (System.Collections.Generic.KeyNotFoundException)
            {
                ParseError = "[I couldn't understand that sentence.]\r\n";
                return false;
            }
            Result.ParsedNoun = validNouns.Find(x => x.Name == Result.Sentence.DirectObject);
            Result.ParsedIndirectObject = validNouns.Find(x => x.Name == Result.Sentence.IndirectObject);
            Result.ParsedVerbPreposition = Result.Sentence.ParsedVerbPreposition;
            Result.ParsedIndirectObjectPreposition = Result.Sentence.ParsedIndirectObjectPreposition;

            if (Result.ParsedNoun == Result.ParsedIndirectObject && Result.ParsedNoun != null)
            {
                ParseError = "[I couldn't understand that sentence.]\r\n";
                return false;
            }

            previousNoun = Result.ParsedNoun;

            return true;
        }

        private void DoParse(String sentence)
        {
            var parsed = new ParsedSentence();

            //drop "THE"
            sentence = sentence.Replace(" THE ", " ");

            parsed.VerbPhrase = FindVerbPhrase(sentence);

            if (parsed.VerbPhrase == null)
            {
                return;
            }

            parsed.DirectObject = FindDirectObject(sentence, parsed.VerbPhrase);
            if (parsed.DirectObject != null)
            {
                parsed.IndirectObject = FindIndirectObject(sentence, parsed.VerbPhrase, parsed.DirectObject);

            }

            Result.Sentence.VerbPhrase = parsed.VerbPhrase;
            Result.Sentence.DirectObject = parsed.DirectObject;
            Result.Sentence.IndirectObject = parsed.IndirectObject;
        }

        #region parser phrase finders
        private String FindVerbPhrase(String sentence)
        {
            string[] split_sentence = sentence.Split(' ');
            int valid_noun_index = 0;
            bool found_noun = false;

            foreach(string word in split_sentence){
                if(word.Contains(",")){ //ignore words that address someone
                    continue;
                }

                //find the first valid noun in the sentence.
                if (validNouns.FirstOrDefault(x => x.Name.Split(' ')[0] == word) != null)
                {
                    found_noun = true;
                    break;
                }
                valid_noun_index++;
            }

            if (!found_noun)
            {
                //return null;
            }

            //valid_noun_index is now the first word in the sentence that is a valid noun.
            //everything before that should be interpreted as a verb phrase.
            String verb_phrase = "";
            for (int i = 0; i < valid_noun_index; i++)
            {
                verb_phrase += split_sentence[i];
                if (i < valid_noun_index-1)
                {
                    verb_phrase += " ";
                }
            }

            if (prepositions.Contains(verb_phrase.Split()[verb_phrase.Split().Length - 1]))
            {
                Result.Sentence.ParsedVerbPreposition = verb_phrase.Split()[verb_phrase.Split().Length - 1];

                var verb_phrase_split = verb_phrase.Split();
                verb_phrase = string.Join(" ", verb_phrase_split.Take(verb_phrase_split.Length - 1));

            }

            return verb_phrase;
        }

        private String FindDirectObject(String sentence, String verb_phrase)
        {
            String direct_object = "";
            //the direct object is after the verb phrase and before the indirect object.
            //the direct object and indirect object are separated by a preposition.

            string[] split_sentence = sentence.Split(' ');
            string[] split_verb_phrase = verb_phrase.Split(' ');

            int index_of_direct_object;

            //find the index of the first word of the sentence after the verb's preposition if one exists
            if (Result.Sentence.ParsedVerbPreposition == null)
            {
                index_of_direct_object = split_sentence.ToList<String>().IndexOf(split_verb_phrase.Last()) + 1;
            }
            else
            {
                index_of_direct_object = split_sentence.ToList<String>().IndexOf(split_verb_phrase.Last()) + 2;
            }

            if (index_of_direct_object == split_sentence.Count())
            {
                return null; //no direct object of this sentence
            }

            for (int i = index_of_direct_object; i < split_sentence.Count(); i++)
            {
                if(!prepositions.Contains(split_sentence[i])){
                    direct_object += split_sentence[i];
                    if (i != split_sentence.Count())
                    {
                        direct_object += " ";
                    }
                } else {
                    break;
                }
            }

            return direct_object.TrimEnd(' ');
        }

        private String FindIndirectObject(String sentence, String verb_phrase, String direct_object)
        {
            String indirect_object = "";
            //the indirect object follows the direct object then a preposition.

            string[] split_sentence = sentence.Split(' ');
            string[] split_verb_phrase = verb_phrase.Split(' ');
            string[] split_direct_object = direct_object.Split(' ');

            //find the index of the first word of the sentence after the direct object
            var index_of_indirect_object = split_sentence.ToList<String>().IndexOf(split_direct_object.Last()) + 1;
            if (index_of_indirect_object == split_sentence.Count())
            {
                return null;
            }

            //check that the word after the direct object phrase is a preposition
            if (!prepositions.Contains(split_sentence[index_of_indirect_object]))
            {
                //no preposition, invalid grammar
                return null;
            }

            Result.Sentence.ParsedIndirectObjectPreposition = split_sentence[index_of_indirect_object];

            //try interpreting everything after the direct object as a noun.
            for (int i = index_of_indirect_object + 1; i < split_sentence.Count(); i++)
            {
                indirect_object += split_sentence[i];
                if (i != split_sentence.Count())
                {
                    indirect_object += " ";
                }
            }

            return indirect_object.TrimEnd(' ');
        }
        #endregion
    }
}
