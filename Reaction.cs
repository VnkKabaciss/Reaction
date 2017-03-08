using Rocket.API;
using Rocket.Core.Plugins;
using System;
using System.IO;
using Rocket.Unturned.Chat;
using Rocket.Core.Commands;
using Rocket.Unturned.Player;

namespace vnkk.Reaction
{
    public class Reaction : RocketPlugin<ReactionConfig>
    {
        string word;
        public static Reaction Instance;
        public int lastindex = 0;
        private DateTime calledwritten = DateTime.Now;
        private bool calling = false;
        public DateTime? lastCalled = null;
        public string pluginname = "Reaction";
        public string developer = "VnkKabacis";
        public string version = "1.0";
        public string stage = "Beta";
        public string rocketupdatesite = "https://dev.rocketmod.net/plugins/reaction/";


        public void FixedUpdate()

        {
            print();
        }

        protected override void Load()
        {
            if (Configuration.Instance.reward == "heal")
            {
                Rocket.Core.Logging.Logger.Log("Player with best reaction will be awarded by getting healed!", ConsoleColor.Green);
            }
            else if (Configuration.Instance.reward == "")
            {
                Rocket.Core.Logging.Logger.Log("Player with best reaction will not be awarded at all!", ConsoleColor.Green);
            }
            else
            {
                Rocket.Core.Logging.Logger.Log("Player with best reaction will not be awarded at all!", ConsoleColor.Green);
            }
            Rocket.Core.Logging.Logger.Log(pluginname + " by " + developer + " has been loaded!", ConsoleColor.Green);
            Rocket.Core.Logging.Logger.Log("Plugin version is " + version, ConsoleColor.Green);
            Rocket.Core.Logging.Logger.Log("Plugin is now on " + stage + " stage.", ConsoleColor.Green);
            Rocket.Core.Logging.Logger.Log("If you encounter any bugs, please report them at", ConsoleColor.Green);
            Rocket.Core.Logging.Logger.Log(rocketupdatesite, ConsoleColor.Green);
            
        }
        protected override void Unload()
        {
            Rocket.Core.Logging.Logger.Log(pluginname + " by " + developer + " has been unloaded!", ConsoleColor.Green);
        }
        public void print()
        {
            try
            {
                if (calling == true)
                {
                    if (((DateTime.Now - calledwritten).TotalSeconds > Configuration.Instance.maxtime) && calling == true)
                    {
                        UnturnedChat.Say("No one wrote word " + word + " in time!");
                        calling = false;
                        lastindex++;
                    }
                }
                if (State == Rocket.API.PluginState.Loaded && Configuration.Instance.Words != null && (lastCalled == null || ((DateTime.Now - lastCalled.Value).TotalSeconds > Configuration.Instance.time)))
                {
                    if (lastindex > (Configuration.Instance.Words.Length - 1)) lastindex = 0;
                    {
                        ReactionConfig.Word wordtext = Configuration.Instance.Words[lastindex];
                        word = wordtext.Text;
                        lastCalled = DateTime.Now;
                        callnew();
                    }

                }
            }
            catch (Exception ex)
            {
                Rocket.Core.Logging.Logger.LogException(ex);
            }
        }


        [RocketCommand("rc", "Participate in reaction game.", "<Word|Number>", AllowedCaller.Player)]
        public void Executerc(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length == 1)
            {
                if (calling == true)
                {
                    if (command[0].ToLower() == word)
                    {
                        if (Configuration.Instance.reward == "heal")
                        {
                            UnturnedChat.Say(player.CharacterName + " won the reaction game and got healed!");
                            player.Heal(100);
                        }
                        else if (Configuration.Instance.reward == "")
                        {
                            UnturnedChat.Say(player.CharacterName + " won the reaction game but didn't get rewarded");
                        }
                        else
                        {
                            UnturnedChat.Say(player.CharacterName + " won the reaction game but didn't get rewarded");
                            if (Configuration.Instance.logwarns == true)
                            {
                                Rocket.Core.Logging.Logger.Log(player.CharacterName + " won a reaction game because there is no such reward type as" + Configuration.Instance.reward + " !");
                                Rocket.Core.Logging.Logger.Log("If you don't want your players to get rewarded, you should rather leave it empty.");
                                Rocket.Core.Logging.Logger.Log("Or if you did it by mistake, you should go on " + rocketupdatesite + "and check the possibilities for Configuration.");
                            }
                                
                            
                        }
                        calling = false;
                        lastindex++;
                    }
                    else
                    {
                        UnturnedChat.Say(player, "It is not the word that you should write!");
                    }
                }
                else
                {
                    UnturnedChat.Say(player, "Reaction game is not running right now!");
                }
            }
            else
            {
                UnturnedChat.Say(player, "Invalid parameters! Instead try: /rc <Word|Number>");
            }
        }
        [RocketCommand("newword", "Start a new reaction game!", "", AllowedCaller.Both)]
        public void Executenewword(IRocketPlayer caller, string[] command)
        {
            if (calling == true)
            {
                Rocket.Core.Logging.Logger.Log("A reaction game cannot be run while one is already running!");
            }
            else
            {
                callnew();
            }
        }

        public void callnew()
        {
            calledwritten = DateTime.Now;
            UnturnedChat.Say("Write word " + word + " first!");
            calling = true;
        }
    }
            

    }


