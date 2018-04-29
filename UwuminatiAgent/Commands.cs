using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Windows;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Net;
using Newtonsoft.Json;

namespace UwuminatiAgent
{
    public class Commands
    {
        // Start by declaring some necessary fields. 
        
        // First, the directory containing the bot's text and other files.
        string botFileDirectory = @"C:\Users\Public\BotFiles\";
        
        //A series of text file names for storing user entries.
        // moodFile is used for the Big Moods only.
        string moodFile = "BigMoods.txt";

        // aesthFile is used for Aesthetics only.
        string aesthFile = "Aesthetic.txt";

        // yandFile is used for Adam's Yanderes only. 
        string yandFile = "Adam.txt";

        // smugFile is used for Adam's Smugs only.
        string smugFile = "SmugGirls.txt";

        // aheFile is used for Adam's Ahegaos only.
        string aheFile = "Ahegao.txt";

        // reactFile is used for Reactions only.
        string reactFile = "Reactions.txt";

        // faceFile is used for MFWs only.
        string faceFile = "MFW.txt";

        // quoteFile is for Quotes.
        string quoteFile = "Quotes.txt";

        // botMoodFile is for BotMoods.
        string botMoodFile = "BotMoods.txt";

        // Init our pseudo-random number generator on the bot's launch.
        Random PRNG = new Random();

        // Just going to go ahead and store this here.
        string randomImgur = "https://imgur.com/random";

        // All 140 champions in League of Legends as of 4/26/2018.
        String[] LeagueChamps = new String[] {"Aatrox","Ahri","Akali","Alistar","Amumu","Anivia","Annie","Ashe","Aurelion Sol","Azir","Bard",
            "Blitzcrank","Brand","Braum","Caitlyn","Camille","Cassiopeia","Cho'Gath","Corki","Darius","Diana","Dr. Mundo","Draven","Ekko",
            "Elise","Evelynn","Ezreal","Fiddlesticks","Fiora","Fizz","Galio","Gangplank","Garen","Gnar","Gragas","Graves","Hecarim",
            "Heimerdinger","Illaoi","Irelia","Ivern","Janna","Jarvan IV","Jax","Jayce","Jhin","Jinx","Kai'sa","Kalista","Karma",
            "Karthus","Kassadin","Katarina","Kayle","Kayn","Kennen","Kha'Zix","Kindred","Kled","Kog'Maw","LeBlanc","Lee Sin","Leona",
            "Lissandra","Lucian","Lulu","Lux","Malphite","Malzahar","Maokai","Master Yi","Miss Fortune","Mordekaiser","Morgana","Nami",
            "Nasus","Nautilus","Nidalee","Nocturne","Nunu","Olaf","Orianna","Ornn","Pantheon","Poppy","Quinn","Rakan","Rammus","Rek'sai",
            "Renekton","Rengar","Riven","Rumble","Ryze","Sejuani","Shaco","Shen","Shyvana","Singed","Sion","Sivir","Skarner","Sona","Soraka",
            "Swain","Syndra","Tahm Kench","Taliyah","Talon","Taric","Teemo","Thresh","Tristana","Trundle","Tryndamere","Twisted Fate",
            "Twitch","Udyr","Urgot","Varus","Vayne","Veigar","Vel'koz","Vi","Viktor","Vladimir","Volibear","Warwick","Wukong","Xayah","Xerath",
            "Xin Zhao","Yasuo","Yorick","Zac","Zed","Ziggs","Zilean","Zoe","Zyra"};

        // And now the subjective role lists for League.

        // Top Laners
        String[] LeagueTops = new String[] {"Aatrox","Camille","Cho'Gath","Darius","Dr. Mundo","Fiora", "Gangplank", "Garen", "Gnar", "Illaoi", "Irelia",
            "Jarvan IV", "Jax", "Jayce", "Kayle", "Kennen", "Kled", "Lissandra", "Malphite", "Maokai", "Mordekaiser", "Nasus", "Nautilus",
            "Olaf","Ornn","Pantheon","Poppy","Quinn","Renekton","Riven","Rumble","Shen","Singed","Sion","Swain","Tahm Kench","Teemo","Trundle",
            "Tryndamere","Urgot","Vi","Vladimir","Volibear","Warwick","Wukong","Yasuo","Yorick"};

        // Junglers
        String[] LeagueJungles = new String[] {"Aatrox","Amumu","Cho'Gath","Diana","Dr. Mundo","Elise","Evelynn","Fiddlesticks","Gragas","Graves","Hecarim","Ivern",
            "Jarvan IV","Jax","Kayn","Kha'zix","Kindred","Lee Sin","Maokai","Master Yi","Nautilus","Nidalee","Nocturne","Nunu","Olaf","Poppy",
            "Rammus","Rek'sai","Rengar","Shaco","Shyvana","Sion","Skarner","Trundle","Tryndamere","Udyr","Vi","Volibear","Warwick","Xin Zhao","Zac"};

        // Mid Laners
        String[] LeagueMids = new String[] {"Ahri","Akali","Anivia","Annie","Aurelion Sol","Azir","Brand","Cassiopeia","Corki","Diana","Ekko","Fiddlesticks","Fizz","Galio",
            "Heimerdinger","Jayce","Karma","Karthus","Kassadin","Katarina","LeBlanc","Lissandra","Lux","Malzahar","Orianna","Ryze","Swain","Syndra",
            "Taliyah","Talon","Twisted Fate","Veigar","Vel'koz","Viktor","Vladimir","Xerath","Yasuo","Zed","Zilean","Zoe"};

        // ADCs
        String[] LeagueADCs = new String[] {"Ashe","Caitlyn","Corki","Draven","Ezreal","Jhin","Jinx","Kai'sa","Kalista","Lucian","Miss Fortune","Sivir","Tristana","Twitch",
            "Varus","Vayne","Xayah"};

        // Supports
        String[] LeagueSupports = new String[] {"Alistar","Bard","Blitzcrank","Brand","Braum","Fiddlesticks","Janna","Karma","Leona","Lulu","Lux","Morgana","Nami",
            "Nautilus","Rakan","Sion","Sona","Soraka","Tahm Kench","Taric","Thresh","Zilean","Zyra"};
        

        // Hardcoded Command List
        // TODO: Use Discord's built in help formatting to make this less necessary.
        // The option to list commands is desperately important.
        [Command("listcommands")]
        public async Task ListCommands(CommandContext ctx)
        {
            await ctx.RespondAsync($"```To get my attention, start commands with '!', uwu\n" +
                $"Also, any and all lists should include mostly image links. You know why, uwu: \n\n\n" +
                $"RELATIVELY BENIGN AND UTILITY COMMANDS" +
                $"'hi': Hello.\n" +
                $"'random min max': Generate a random number between the min and max.\n" +
                $"'catters': Returns a cat image from random.cat!\n" +
                $"'setalarm time|message': Set an alarm with a specified direct message to be sent to you. Time is in minutes.\n\n\n" +
                $"MEME STUFF AND CALL-RESPONSE STUFF\n" +
                $"'addreaction': Add a Reaction to the list.\n" +
                $"'reaction': Get a reaction to the situation.\n" +
                $"'addmfw': Add your face when to the list.\n" +
                $"'mfw': Share how you must REALLY look right now.\n" +
                $"'addmood': Adds a Big Mood to the list.\n" +
                $"'bigmood': Find out what the Big Mood is.\n" +
                $"'addaesthetic': Adds an Aesthetic to the list. This can(and should) include image urls.\n" +
                $"'aesthetic': What's your Aesthetic right now?\n" +
                $"'addquote': Add the message above yours to the quote list.\n" +
                $"'quote': Get a random quote from the pages of history.\n" +
                $"'addbotmood': Add a new mood for the bot's emotional expression.\n" +
                $"'botmood': The bot will tell you how it's really feeling.\n\n\n" +
                $"LEAGUE OF LEGENDS COMMANDS\n" +
                $"'fill': Autofill. Picks your champion for you from all of them.\n" +
                $"'toplane': Top Lane Champions only.\n" +
                $"'jungle': Jungle Champions only.\n" +
                $"'midlane': Mid Lane Champions only.\n" +
                $"'adc': ADC Champions only.\n" +
                $"'support': Support Champions only.\n\n\n" +
                $"ADAM'S OWN CORNER\n" +
                $"'addyandere': Adds a Yandere. This is exclusively for @uwuminati#5179 and nobody else.\n" +
                $"'yandere': Retrieves a Yandere. This is exclusively for @uwuminati#5179 and nobody else.\n" +
                $"'addsmug': Adds a Smug Girl. This is exclusively for @uwuminati#5179 and nobody else.\n" +
                $"'smug': Retreives a Smug Girl. This is exclusively for @uwuminati#5179 and nobody else.\n" +
                $"'addahegao': Adds an Ahegao. This is exclusively for @uwuminati#5179 and nobody else.\n" +
                $"'ahegao': Retreives an Ahegao. This is excluseively for @uwuminati#5179 and nobody else.```");
        }

        // =========================================
        // Utility and generic commands
        // =========================================

        // Default "hello" command, also useful as a hello world.
        [Command("hi")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}, uwu");
        }

        // Every bot needs a random number function, I guess?
        [Command("random")]
        public async Task Random(CommandContext ctx, int min, int max)
        {
            await ctx.RespondAsync($"{ctx.User.Mention} rolls between {min} and {max} and gets {PRNG.Next(min, max)}");
        }

        // Cats!
        [Command("catters")]
        public async Task Catters(CommandContext ctx)
        {
            // Instantiate a new Webclient. Very exciting.
            using (WebClient wc = new WebClient())
            {
                // Save and deserialize the JSON response.
                var catJson = wc.DownloadString("http://aws.random.cat/meow");
                var catResponse = JsonConvert.DeserializeObject<CatResponse>(catJson);

                // Reply with the deserialized response.
                await ctx.RespondAsync(catResponse.CatUrl);
            }
        }

        // Object to hold the Cats. I'll have to see if there's a better way to do this.
        public struct CatResponse
        {
            [JsonProperty("file")]
            public string CatUrl { get; private set; }
        }

        // ===========================================
        // Alarm function-related commands go here:
        // ===========================================
        [Command("setalarm")]
        public async Task SetAlarm(CommandContext ctx)
        {
            // Get the user that sent the command.
            var alarmUser = ctx.Message.Author.Id;
            // Do some processing to separate the timer from the message.
            int divider = ctx.Message.Content.IndexOf("|");
            int subLength = divider - 10;

            // Test the input properly.
            try
            {
                ulong.Parse(ctx.Message.Content.Substring(10, subLength));
            }
            catch (FormatException)
            {
                await ctx.RespondAsync("That's not quite right. Make sure there's only one space between !setalarm and time, and your time is in numeric format.");
            }

            // Get the time specified.
            ulong minutes = Convert.ToUInt64(ctx.Message.Content.Substring(10, subLength));


            // Set the message to a variable for later use.
            string message = ctx.Message.Content.Substring(divider + 1);

            // Set the timer and start it.
            ulong alarmTime = minutes * 60000;
            Timer timer = new Timer(alarmTime);
            timer.Elapsed += (sender, e) => SendAlarmMessage(timer, sender, e, ctx, message);
            timer.Enabled = true;

            // Inform the user the command was successful.
            await ctx.RespondAsync($"Alarm set, {ctx.User.Mention}");
        }

        // Once the timer is finished, it calls this function, which returns the message specified.
        public async void SendAlarmMessage(Timer selfTimer, Object source, ElapsedEventArgs e, CommandContext ctx, string userMessage)
        {
            await ctx.Member.SendMessageAsync(userMessage);
            selfTimer.Dispose();
        }
        
        // =======================================
        // Memey stuff and call-response
        // =======================================

        // AddMood allows the user to add a Big Mood to a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of Moods (moodFile). These are stored as strings in plain text, as security is not an issue here.
        [Command("addmood")]
        public async Task AddMood(CommandContext ctx, string link)
        {
            using (StreamWriter File = new StreamWriter(botFileDirectory + moodFile, true))
            {
                string userMood = ctx.Message.Content.Substring(9);
                await File.WriteLineAsync(userMood.Trim());
            }
                await ctx.RespondAsync($"Big mood, {ctx.User.Mention}");
        }

        // BigMood allows the user to recover a Big Mood at random from a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of Moods (moodFile). The random line is found using an inefficient algorithm, as it is (in this case)
        // impossible to avoid copying the entire file into memory to count the lines.
        [Command("bigmood")]
        public async Task BigMood(CommandContext ctx)
        {
            string moodFilePath = botFileDirectory + moodFile;

            // Thankfully our file is guaranteed to be plaintext by the nature of the beast, but this is still a horrifically inefficent solution, 
            // even if it is the only one available.
            var moodFileArray = File.ReadAllLines(moodFilePath);
            string theMood = moodFileArray[PRNG.Next(0, moodFileArray.Length)];
            await ctx.RespondAsync($"{theMood}");
        }

        // AddAesthetic allows the user to add an Aesthetic to a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of Aesthetics (aesthFile). These are stored as strings in plain text, as security is not an issue here.
        [Command("addaesthetic")]
        public async Task AddAesthetic(CommandContext ctx, string link)
        {
            using (StreamWriter File = new StreamWriter(botFileDirectory + aesthFile, true))
            {
                string userAesth = ctx.Message.Content.Substring(14);
                await File.WriteLineAsync(userAesth.Trim());
            }
            await ctx.RespondAsync($"That's my aesthetic, {ctx.User.Mention}");
        }

        // Aesthetic allows the user to recover an Aesthetic at random from a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of Aesthetics (aesthFile). The random line is found using an inefficient algorithm, as it is (in this case)
        // impossible to avoid copying the entire file into memory to count the lines.
        [Command("aesthetic")]
        public async Task Aesthetic(CommandContext ctx)
        {
            string aesthFilePath = botFileDirectory + aesthFile;

            // Thankfully our file is guaranteed to be plaintext by the nature of the beast, but this is still a horrifically inefficent solution, 
            // even if it is the only one available.
            var aesthFileArray = File.ReadAllLines(aesthFilePath);
            string myAesth = aesthFileArray[PRNG.Next(0, aesthFileArray.Length)];
            await ctx.RespondAsync($"{myAesth}");
        }

        // AddYandere allows Adam to add a Yandere to a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of Yandere (yandFile). These are stored as strings in plain text, as security is not an issue here.
        [Command("addyandere")]
        public async Task AddYandere(CommandContext ctx, string link)
        {
            if (ctx.User.Discriminator.ToString().Equals("5179"))
            {
                using (StreamWriter File = new StreamWriter(botFileDirectory + yandFile, true))
                {
                    string userYand = ctx.Message.Content.Substring(12);
                    await File.WriteLineAsync(userYand.Trim());
                }
                await ctx.RespondAsync($"Added to the list, {ctx.User.Mention}");
            }
            else
            {
                await ctx.RespondAsync($"You do not have permission to use this command.");
            }
        }

        // GetYandere allows Adam to recover a Yandere at random from a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of Yanderes (yandFile). The random line is found using an inefficient algorithm, as it is (in this case)
        // impossible to avoid copying the entire file into memory to count the lines.
        [Command("yandere")]
        public async Task GetYandere(CommandContext ctx)
        {
            if (ctx.User.Discriminator.ToString().Equals("5179"))
            {
                string yandFilePath = botFileDirectory + yandFile;

                // Thankfully our file is guaranteed to be plaintext by the nature of the beast, but this is still a horrifically inefficent solution, 
                // even if it is the only one available.
                var yandFileArray = File.ReadAllLines(yandFilePath);
                string adamYandere = yandFileArray[PRNG.Next(0, yandFileArray.Length)];
                await ctx.RespondAsync($"{adamYandere}");
            }
            else
            {
                await ctx.RespondAsync($"You do not have permission to use this command.");
            }
        }

        // AddSmug allows Adam to add a Smug Girl to a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of Smug Girls (smugFile). These are stored as strings in plain text, as security is not an issue here.
        [Command("addsmug")]
        public async Task AddSmug(CommandContext ctx, string link)
        {
            if (ctx.User.Discriminator.ToString().Equals("5179"))
            {
                using (StreamWriter File = new StreamWriter(botFileDirectory + smugFile, true))
                {
                    string userSmug = ctx.Message.Content.Substring(9);
                    await File.WriteLineAsync(userSmug.Trim());
                }
                await ctx.RespondAsync($"Added to the list, {ctx.User.Mention}");
            }
            else
            {
                await ctx.RespondAsync($"You do not have permission to use this command.");
            }
        }

        // GetSmug allows Adam to recover a Smug Girl at random from a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of Smug Girls (smugFile). The random line is found using an inefficient algorithm, as it is (in this case)
        // impossible to avoid copying the entire file into memory to count the lines.
        [Command("smug")]
        public async Task GetSmug(CommandContext ctx)
        {
            if (ctx.User.Discriminator.ToString().Equals("5179"))
            {
                string smugFilePath = botFileDirectory + smugFile;

                // Thankfully our file is guaranteed to be plaintext by the nature of the beast, but this is still a horrifically inefficent solution, 
                // even if it is the only one available.
                var smugFileArray = File.ReadAllLines(smugFilePath);
                string adamSmug = smugFileArray[PRNG.Next(0, smugFileArray.Length)];
                await ctx.RespondAsync($"{adamSmug}");
            }
            else
            {
                await ctx.RespondAsync($"You do not have permission to use this command.");
            }
        }

        // AddAhegao allows Adam to add an Ahegao to a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of Ahegao (aheFile). These are stored as strings in plain text, as security is not an issue here.
        [Command("addahegao")]
        public async Task AddAhegao(CommandContext ctx, string link)
        {
            if (ctx.User.Discriminator.ToString().Equals("5179"))
            {
                using (StreamWriter File = new StreamWriter(botFileDirectory + aheFile, true))
                {
                    string userAhe = ctx.Message.Content.Substring(11);
                    await File.WriteLineAsync(userAhe.Trim());
                }
                await ctx.RespondAsync($"Added to the list, {ctx.User.Mention}");
            }
            else
            {
                await ctx.RespondAsync($"You do not have permission to use this command.");
            }
        }

        // GetAhegao allows Adam to recover an Ahegao at random from a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of Ahegaos (aheFile). The random line is found using an inefficient algorithm, as it is (in this case)
        // impossible to avoid copying the entire file into memory to count the lines.
        [Command("ahegao")]
        public async Task GetAhegao(CommandContext ctx)
        {
            if (ctx.User.Discriminator.ToString().Equals("5179"))
            {
                string aheFilePath = botFileDirectory + aheFile;

                // Thankfully our file is guaranteed to be plaintext by the nature of the beast, but this is still a horrifically inefficent solution, 
                // even if it is the only one available.
                var aheFileArray = File.ReadAllLines(aheFilePath);
                string adamAhegao = aheFileArray[PRNG.Next(0, aheFileArray.Length)];
                await ctx.RespondAsync($"{adamAhegao}");
            }
            else
            {
                await ctx.RespondAsync($"You do not have permission to use this command.");
            }
        }

        // AddReaction allows the user to add a Reaction to a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of Reactions (reactFile). These are stored as strings in plain text, as security is not an issue here.
        [Command("addreaction")]
        public async Task AddReaction(CommandContext ctx, string link)
        {
            using (StreamWriter File = new StreamWriter(botFileDirectory + reactFile, true))
            {
                string userReact = ctx.Message.Content.Substring(13);
                await File.WriteLineAsync(userReact.Trim());
            }
            await ctx.RespondAsync($"Reaction stored, {ctx.User.Mention}");
        }

        // Reaction allows the user to recover a reaction at random from a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of Aesthetics (reactFile). The random line is found using an inefficient algorithm, as it is (in this case)
        // impossible to avoid copying the entire file into memory to count the lines.
        [Command("reaction")]
        public async Task Reaction(CommandContext ctx)
        {
            string reactFilePath = botFileDirectory + reactFile;

            // Thankfully our file is guaranteed to be plaintext by the nature of the beast, but this is still a horrifically inefficent solution, 
            // even if it is the only one available.
            var reactFileArray = File.ReadAllLines(reactFilePath);
            string react = reactFileArray[PRNG.Next(0, reactFileArray.Length)];
            await ctx.RespondAsync($"{react}");
        }

        // AddMFW allows the user to add an Aesthetic to a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of Aesthetics (aesthFile). These are stored as strings in plain text, as security is not an issue here.
        [Command("addmfw")]
        public async Task AddMFW(CommandContext ctx, string link)
        {
            using (StreamWriter File = new StreamWriter(botFileDirectory + faceFile, true))
            {
                string userMFW = ctx.Message.Content.Substring(8);
                await File.WriteLineAsync(userMFW.Trim());
            }
            await ctx.RespondAsync($"That's a good face, {ctx.User.Mention}");
        }

        // MFW allows the user to recover a MFW at random from a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of MFWs (faceFile). The random line is found using an inefficient algorithm, as it is (in this case)
        // impossible to avoid copying the entire file into memory to count the lines.
        [Command("mfw")]
        public async Task MFW(CommandContext ctx)
        {
            string faceFilePath = botFileDirectory + faceFile;

            // Thankfully our file is guaranteed to be plaintext by the nature of the beast, but this is still a horrifically inefficent solution, 
            // even if it is the only one available.
            var faceFileArray = File.ReadAllLines(faceFilePath);
            string myFace = faceFileArray[PRNG.Next(0, faceFileArray.Length)];
            await ctx.RespondAsync($"{myFace}");
        }
        
        //addquote allows the user to save the immediate previous message to a locally stored file.
        [Command("addquote")]
        public async Task AddQuote(CommandContext ctx)
        {
            string quoteFilePath = botFileDirectory + quoteFile;

            var messagesToObtain = await ctx.Channel.GetMessagesAsync(2);

            // Get the user's ID...
            ulong quoteUserId = messagesToObtain[1].Author.Id;
            // ...so that we can track down their nickname...
            var quoteUser = await ctx.Guild.GetMemberAsync(quoteUserId);
            // ...because the only endpoint for nicknames is via Guilds.

            // Save the formatted quote.
            string quote = messagesToObtain[1].Timestamp.LocalDateTime + " **" + quoteUser.Nickname + "**: " + messagesToObtain[1].Content.ToString();

            using (StreamWriter File = new StreamWriter(botFileDirectory + quoteFile, true))
            {
                await File.WriteLineAsync(quote);
                await ctx.RespondAsync($"Quote saved for all eternity.");
            }
        }

        // Quote allows the user to recover a quote at random from a locally stored file, determined by taking the global bot directory (botFileDirectory)
        // and concatenating the location of the list of quotes (quoteFile). The random line is found using an inefficient algorithm, as it is (in this case)
        // impossible to avoid copying the entire file into memory to count the lines.
        [Command("quote")]
        public async Task Quote(CommandContext ctx)
        {
            string quoteFilePath = botFileDirectory + quoteFile;

            // Thankfully our file is guaranteed to be plaintext by the nature of the beast, but this is still a horrifically inefficent solution, 
            // even if it is the only one available.
            var quoteFileArray = File.ReadAllLines(quoteFilePath);
            string quote = quoteFileArray[PRNG.Next(0, quoteFileArray.Length)];
            string timestamp = quote.Substring(0, quote.IndexOf("*"));
            string quoteText = quote.Substring(quote.IndexOf("*"));
            await ctx.RespondAsync($"{timestamp} \n {quoteText}");
        }

        // The Bot's moods change like the wind...
        [Command("addbotmood")]
        public async Task AddBotMood(CommandContext ctx)
        {
            using (StreamWriter File = new StreamWriter(botFileDirectory + botMoodFile, true))
            {
                string newBotMood = ctx.Message.Content.Substring(12);
                await File.WriteLineAsync(newBotMood);
            }
            await ctx.RespondAsync($"I feel that, {ctx.User.Mention}");
        }

        // ...who can possibly predict them?
        [Command("botmood")]
        public async Task BotMood(CommandContext ctx)
        {
            string botMoodFilePath = botFileDirectory + botMoodFile;

            // Thankfully our file is guaranteed to be plaintext by the nature of the beast, but this is still a horrifically inefficent solution, 
            // even if it is the only one available.
            var botMoodFileArray = File.ReadAllLines(botMoodFilePath);
            string botMood = botMoodFileArray[PRNG.Next(0, botMoodFileArray.Length)];
            await ctx.RespondAsync($"{botMood}");
        }

        // ============================================
        // HERE BEGIN THE LEAGUE COMMANDS
        // ============================================

        // True Fill, for the authentic ARAM experience
        [Command("fill")]
        public async Task TrueFill(CommandContext ctx)
        {
            await ctx.RespondAsync($"{ctx.User.Mention} should play {LeagueChamps[PRNG.Next(0, LeagueChamps.Length)]}");
        }
        // Top Lane, for the solo pvp island
        [Command("toplane")]
        public async Task TopLane(CommandContext ctx)
        {
            await ctx.RespondAsync($"{ctx.User.Mention} should play {LeagueTops[PRNG.Next(0, LeagueTops.Length)]}");
        }
        // Jungle, for when you just want to play WoW
        [Command("jungle")]
        public async Task Jungle(CommandContext ctx)
        {
            await ctx.RespondAsync($"{ctx.User.Mention} should play {LeagueJungles[PRNG.Next(0, LeagueJungles.Length)]}");
        }
        // Mid Lane, for the esports highlight reels
        [Command("midlane")]
        public async Task MidLane(CommandContext ctx)
        {
            await ctx.RespondAsync($"{ctx.User.Mention} should play {LeagueMids[PRNG.Next(0, LeagueMids.Length)]}");
        }
        // ADC, for the inferiority complex stricken
        [Command("adc")]
        public async Task ADC(CommandContext ctx)
        {
            await ctx.RespondAsync($"{ctx.User.Mention} should play {LeagueADCs[PRNG.Next(0, LeagueADCs.Length)]}");
        }
        // True Fill, for the authentic ARAM experience
        [Command("support")]
        public async Task Support(CommandContext ctx)
        {
            await ctx.RespondAsync($"{ctx.User.Mention} should play {LeagueSupports[PRNG.Next(0, LeagueSupports.Length)]}");
        }
    }
}
