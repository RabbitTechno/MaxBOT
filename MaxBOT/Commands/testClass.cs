using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Lavalink;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace MaxBOT
{
    internal class testClass : BaseCommandModule
    {
        
        
                      ///  First way to make an embed
        [Command("Embed")]
        public async Task Embed(CommandContext ctx)
        {
            var messege = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder()
                .WithTitle("Hi this is Max")
                .WithDescription("a bot made by a devoloper called Rabbit")
                .WithImageUrl("")
                .WithColor(DiscordColor.Aquamarine));

            await ctx.Channel.SendMessageAsync(messege);
        }
      

        /// ///////////////////////////////////////////////////////////////////
        // the SECOND WAY

        [Command("Embed2")]
        public async Task Embed2(CommandContext ctx)
        {
            var messege2 = new DiscordEmbedBuilder {

                Title = "Here you will see better things",
                Color = DiscordColor.Orange,
                Description = "i have nothing to say lol",
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "the icon",
                    IconUrl = "https://cdn-icons-png.flaticon.com/512/3135/3135715.png"
                },
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = "adham"
                }
                ,
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = "https://img.pikbest.com/backgrounds/20190805/blue-purple-cool-fantasy-light-effect-banner-background_2758856.jpg!w700wp"
                }
            };
            await ctx.Channel.SendMessageAsync(embed: messege2);
        }

        //################################################################//################################################################
        //################################################################//################################################################
        //################################################################//################################################################
        [Command("avatar")]
        public async Task stuff(CommandContext ctx, DiscordUser targetUser = null)
        {
            // Access the member who invoked the command
                                              //if yes                                      : if not 
            var member = targetUser != null ? await ctx.Guild.GetMemberAsync(targetUser.Id) : ctx.Member;

            // Access member properties
            var avatarUrl = member.AvatarUrl.Trim();
           
            // Create a message with the member information
            var message = $"**Avatar URL:** {avatarUrl}\n";
            await ctx.RespondAsync(message);
        }
        //################################################################//################################################################
        //################################################################//################################################################
        //################################################################//################################################################
        [Command("play")]
        public async Task playmusic(CommandContext ctx,[RemainingText] string query)
        {
            var userVC = ctx.Member.VoiceState?.Channel; //depending on if the user in a VC or not this Var should cotain a DiscordChannel or NULL ##
            var lavallinkInstance = ctx.Client.GetLavalink();

            //Pre-check


            // if (ctx.Member.VoiceState.Channel == null || userVC == null)
            // {
            //     await ctx.Channel.SendMessageAsync("please enter a Voice Channel in order to start your Command");
            //      return;
            //  }
            if (userVC == null)
            {
                await ctx.Channel.SendMessageAsync("Join a VC please!");
                return;
            }
            if (!lavallinkInstance.ConnectedNodes.Any())
            {
                await ctx.Channel.SendMessageAsync("the connection is not ESTABLISHED!!!");
                    return;
            }

            if (userVC.Type != DSharpPlus.ChannelType.Voice)
            {
                await ctx.Channel.SendMessageAsync("please enter a Valid Voice Channel");
                    return;
            }

            //to connect the CHANNEL 
            ////// to check if LAVA LINK lost connection
            var node = lavallinkInstance.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);   // The connection using this Node 
             
            await node.ConnectAsync(userVC);
            if (conn == null)
            {
                await ctx.Channel.SendMessageAsync("LavaLink Faild to connect!!!");
                return;
            }

            ///// Last step to play the music 
            var searchQuery = await node.Rest.GetTracksAsync(query);
            if(searchQuery.LoadResultType == LavalinkLoadResultType.NoMatches || searchQuery.LoadResultType == LavalinkLoadResultType.LoadFailed ) // Has it return no result ? 
            {
                await ctx.Channel.SendMessageAsync($"Faild to find the music {query}");
                return;
            }

            // we want the first Search result 
             var musicTrack = searchQuery.Tracks.First();
             await conn.PlayAsync(musicTrack);

            var nowplayitEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Green,
                Title = $"Let's shake that butty!!!! in {userVC.Name} channel",
                Description = $"Now playing {musicTrack.Title} \n" + $"Author is : {musicTrack.Author} \n" + $"URL is : {musicTrack.Uri} \n",
            };
            await ctx.Channel.SendMessageAsync(embed: nowplayitEmbed);
          
        }

        
        ///######################################################################################################################################### <summary>
        /// #########################################################################################################################################
   
        [Command("Stop")]

        public async Task pasuemusic(CommandContext ctx)
        {
            var  userVC = ctx.Member.VoiceState.Channel; //depending on if the user in a VC or not this Var should cotain a DiscordChannel or NULL ##
            var lavallinkInstance = ctx.Client.GetLavalink();

            //////////             CHECK AGAIN 
            if (ctx.Member.VoiceState == null || userVC == null)
            {
                await ctx.Channel.SendMessageAsync("please enter a Voice Channel in order to start your Command");
                return;
            }
            if (!lavallinkInstance.ConnectedNodes.Any())
            {
                await ctx.Channel.SendMessageAsync("the connection is not ESTABLISHED!!!");
                return;
            }

            if (userVC.Type != DSharpPlus.ChannelType.Voice)
            {
                await ctx.Channel.SendMessageAsync("please enter a Valid Voice Channel");
                return;
            }
            
            ///////////////////////////////////////// Code for pause ///////////////////////////////////////////////////////////////
            var node = lavallinkInstance.ConnectedNodes.Values.First();        // The node 
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);   // The connection using this Node 

            // Check if LavaLink is playing something or not YK!!!!
            if (conn == null)
            {
                await ctx.Channel.SendMessageAsync($"LavaLink Faild to connect!!!");
                return;
            }
            if (conn.CurrentState.CurrentTrack == null)  // if there is nothing playing 
            {
                await ctx.Channel.SendMessageAsync("No track is playing");
                    return;
            }
            await conn.PauseAsync();

            var pauseEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Green,
                Title = "Paused",
                Description = "waiting for your command"

            };
            await ctx.Channel.SendMessageAsync(pauseEmbed);
         }
        //################################################################
        //################################################################
        //################################################################
        [Command("Resume")]

        public async Task resum(CommandContext ctx)
        {
            var userVC = ctx.Member.VoiceState.Channel; //depending on if the user in a VC or not this Var should cotain a DiscordChannel or NULL ##
            var lavallinkInstance = ctx.Client.GetLavalink();

            //////////             CHECK AGAIN 
            if (ctx.Member.VoiceState == null || userVC == null)
            {
                await ctx.Channel.SendMessageAsync("please enter a Voice Channel in order to start your Command");
                return;
            }
            if (!lavallinkInstance.ConnectedNodes.Any())
            {
                await ctx.Channel.SendMessageAsync("the connection is not ESTABLISHED!!!");
                return;
            }

            if (userVC.Type != DSharpPlus.ChannelType.Voice)
            {
                await ctx.Channel.SendMessageAsync("please enter a Valid Voice Channel");
                return;
            }

            ///////////////////////////////////////// Code for pause ///////////////////////////////////////////////////////////////
            var node = lavallinkInstance.ConnectedNodes.Values.First();        // The node 
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);   // The connection using this Node 

            // Check if LavaLink is playing something or not YK!!!!
            if (conn == null)
            {
                await ctx.Channel.SendMessageAsync($"LavaLink Faild to connect!!!");
                return;
            }
            if (conn.CurrentState.CurrentTrack == null)  // if there is nothing playing 
            {
                await ctx.Channel.SendMessageAsync("No track is playing");
                return;
            }
            await conn.ResumeAsync();

            var pauseEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Green,
                Title = "resumed",
              

            };
            await ctx.Channel.SendMessageAsync(pauseEmbed);
        }

        //################################################################
        //################################################################
        //################################################################
        //################################################################

        [Command("Leave")]

        public async Task leave(CommandContext ctx)
        {
            var userVC = ctx.Member.VoiceState.Channel; //depending on if the user in a VC or not this Var should cotain a DiscordChannel or NULL ##
            var lavallinkInstance = ctx.Client.GetLavalink();

            //////////             CHECK AGAIN 
           //if (ctx.Member.VoiceState == null || userVC == null)
           // {
          //      await ctx.Channel.SendMessageAsync("please enter a Voice Channel in order to start your Command");
           //     return;
          //  }
           // var userVC2 = ctx.Member.VoiceState?.Channel;
            if (userVC == null)
            {
                await ctx.Channel.SendMessageAsync("You need to be in a voice channel to use this command!");
                return;
            }

            if (!lavallinkInstance.ConnectedNodes.Any())
            {
                await ctx.Channel.SendMessageAsync("the connection is not ESTABLISHED!!!");
                return;
            }

            if (userVC.Type != DSharpPlus.ChannelType.Voice)
            {
                await ctx.Channel.SendMessageAsync("please enter a Valid Voice Channel");
                return;
            }

            ///////////////////////////////////////// Code for pause ///////////////////////////////////////////////////////////////
            var node = lavallinkInstance.ConnectedNodes.Values.First();        // The node 
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);   // The connection using this Node 

            // Check if LavaLink is playing something or not YK!!!!
            if (conn == null)
            {
                await ctx.Channel.SendMessageAsync($"Faild to connect!!!");
                return;
            }
          
            await conn.DisconnectAsync();

            var pauseEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Green,
                Title = "Disconnected",
               
            };
            await ctx.Channel.SendMessageAsync(pauseEmbed);
        }

        //################################################################
        //################################################################
        //################################################################
        //################################################################

        [Command("Help")]
        public async Task help(CommandContext ctx)
        {
            var help = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Green,
                Title = "Help",
                Description = $"The Bot Commands is the following : \n" + $"1- Avatar : To display a person PFP or your PFP\n" + $"2- Play : to play a song you want from Youtube \n" + $"3- Stop : to pause the song \n" + $"4- Resume : to resume the song \n" + $"5- Leave : to disconnect from the call \n\n" + $"Additional Features \n" + $"if you want to change the Bot NAME & Picture to whatever you want ?? \n" +$"Please contact the admin of the link in the Bot's Bio \n"

            };
            await ctx.Channel.SendMessageAsync(embed: help);
        }

        //################################################################
        //################################################################
        //################################################################
        //################################################################

        [Command("test")]
        public async Task inter(CommandContext ctx)
        {

            var interActivity = Program.Client.GetInteractivity();

            var messegeToRetrive = await interActivity.WaitForMessageAsync(messege => messege.Content == "Hello");


            if (messegeToRetrive.Result.Content == "Hello")
            {
                await ctx.Channel.SendMessageAsync($"{ctx.User.Username} said Hello");
            }
        }
    }
}
