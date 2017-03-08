using Rocket.API;
using System.Xml.Serialization;

namespace vnkk.Reaction
{
    public class ReactionConfig : IRocketPluginConfiguration
    {

        public sealed class Word
        {
            [XmlAttribute("Word")]
            public string Text;


            public Word(string text)
            {

                Text = text;
            }
            public Word()
            {
                Text = "";
            }
        }
        public string reward;
        public int time;
        public int maxtime;
        public bool logwarns;
        [XmlArrayItem("Word")]
        [XmlArray(ElementName = "Words")]
        public Word[] Words;


        public void LoadDefaults()
        {
            Words = new Word[]
            {
                new Word("potato"),
                new Word("pepper"),
                new Word("finger"),
                new Word("table"),
                new Word("earth"),
                new Word("cucumber")
            };
            reward = "heal";
            time = 300;
            maxtime = 10;
            logwarns = true;
        }
    }
}
