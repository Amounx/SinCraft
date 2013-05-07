/********************************************************************************************************\
<+> Copyright 2013 Sinjai                                                                              
<+>                                                                                                    
<+> licensed under a Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.         
<+> http://creativecommons.org/licenses/by-nc-nd/3.0/                                                  
<+>                                                                                                    
<+> This command was made for use with MCForge only.                                                   
<+> You must attribute the work in the manner specified by the author or licensor.                     
<+> You may not use this work for commercial purposes.                                                 
<+> You may not alter, transform, or build upon this work.                                             
<+>                                                                                                    
<+> Any of the above conditions can be waived if you get written permission from the copyright holder.
\********************************************************************************************************/
using System;
using System.Data;
using SinCraft.SQL;

namespace SinCraft.Commands
{
    public sealed class CmdInfo : Command
    {
        public override string name { get { return "info"; } }
        public override string[] aliases { get { return new string[] { "information", "whois", "whowas", "whoip" }; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdInfo() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (message.IndexOf("'") != -1) { Player.SendMessage(p, "Cannot parse request."); return; }
            #region Whoip
            if (message.Split(' ')[0].ToLower() == "ip")
            {
                if (p.group.Permission < LevelPermission.Operator) { Player.SendMessage(p, "You cannot check IPs."); return; }
                Database.AddParams("@IP", message.Split(' ')[1]);
                DataTable playerDb = Database.fillData("SELECT Name FROM Players WHERE IP=@IP");
                if (playerDb.Rows.Count == 0) { Player.SendMessage(p, "Could not find anyone with this IP"); return; }
                string playerNames = "Players with this IP: ";
                for (int i = 0; i < playerDb.Rows.Count; i++)
                {
                    playerNames += playerDb.Rows[i]["Name"] + ", ";
                }
                playerNames = playerNames.Remove(playerNames.Length - 2);
                Player.SendMessage(p, playerNames);
                playerDb.Dispose();
                return;
            }
            #endregion
            #region Whois
            Player who = null;
            if (message == "") { who = p; message = p.Username; } else { who = Player.Find(message); }
            if (who != null && !who.hidden)
            {
                Player.SendMessage(p, who.color + who.Username + Server.DefaultColor + " is on &b" + who.level.name);
                Player.SendMessage(p, who.color + who.Username + Server.DefaultColor + " has their name set to " + who.SetName);
                Player.SendMessage(p, who.color + who.prefix + who.SetName + Server.DefaultColor + " has :");
                Player.SendMessage(p, "> > the rank of " + who.group.color + who.group.name);
                try
                {
                    if (!Group.Find("Nobody").commands.Contains("pay") && !Group.Find("Nobody").commands.Contains("give") && !Group.Find("Nobody").commands.Contains("take")) Player.SendMessage(p, "> > &a" + who.Money + Server.DefaultColor + " " + Server.Currency);
                }
                catch { }
                Player.SendMessage(p, "> > &cdied &a" + who.Deaths + Server.DefaultColor + " times");
                Player.SendMessage(p, "> > &bmodified &a" + who.BlocksModified + " &eblocks &eand &a" + who.loginBlocks + " &ewere changed &9since logging in&e.");
                string storedTime = Convert.ToDateTime(DateTime.Now.Subtract(who.timeLogged).ToString()).ToString("HH:mm:ss");
                Player.SendMessage(p, "> > time spent on server: " + who.TimeSpent.Split(' ')[0] + " Days, " + who.TimeSpent.Split(' ')[1] + " Hours, " + who.TimeSpent.Split(' ')[2] + " Minutes, " + who.TimeSpent.Split(' ')[3] + " Seconds.");
                Player.SendMessage(p, "> > been logged in for &a" + storedTime);
                Player.SendMessage(p, "> > first logged into the server on &a" + who.firstLogin.ToString("yyyy-MM-dd") + " at " + who.firstLogin.ToString("HH:mm:ss"));
                Player.SendMessage(p, "> > logged in &a" + who.Logins + Server.DefaultColor + " times, &c" + who.TimesKicked + Server.DefaultColor + " of which ended in a kick.");
                Player.SendMessage(p, "> > " + Awards.awardAmount(who.Username) + " awards");
                if (Ban.Isbanned(who.Username))
                {
                    string[] data = Ban.Getbandata(who.Username);
                    Player.SendMessage(p, "> > is banned for " + data[1] + " by " + data[0]);
                }

                if (who.isDev)
                {
                    Player.SendMessage(p, Server.DefaultColor + "> > Player is a &9Developer");
                }

                if (!(p != null && (int)p.group.Permission <= CommandOtherPerms.GetPerm(this)))
                {
                    string givenIP;
                    if (Server.bannedIP.Contains(who.ip)) givenIP = "&8" + who.ip + ", which is banned";
                    else givenIP = who.ip;
                    Player.SendMessage(p, "> > the IP of " + givenIP);
                    if (Server.useWhitelist)
                    {
                        if (Server.whiteList.Contains(who.Username))
                        {
                            Player.SendMessage(p, "> > Player is &fWhitelisted");
                        }
                    }
                }
            }
            #endregion
            #region Whowas
            else
            {
                string FoundRank = Group.findPlayer(message.ToLower());
                Database.AddParams("@Name", message);
                DataTable playerDb = Database.fillData("SELECT * FROM Players WHERE Name=@Name");
                if (playerDb.Rows.Count == 0) { Player.SendMessage(p, Group.Find(FoundRank).color + message + Server.DefaultColor + " has the rank of " + Group.Find(FoundRank).color + FoundRank); return; }
                string title = playerDb.Rows[0]["Title"].ToString();
                string color = c.Parse(playerDb.Rows[0]["color"].ToString().Trim());
                if (color == "" || color == null || String.IsNullOrEmpty(color)) color = Group.Find(FoundRank).color;
                string tcolor = c.Parse(playerDb.Rows[0]["title_color"].ToString().Trim());
                if (title == "" || title == null || String.IsNullOrEmpty(title))
                    Player.SendMessage(p, color + message + Server.DefaultColor + " has :");
                else
                    Player.SendMessage(p, color + "[" + tcolor + playerDb.Rows[0]["Title"] + color + "] " + message + Server.DefaultColor + " has :");
                Player.SendMessage(p, "> > the rank of " + Group.Find(FoundRank).color + FoundRank);
                try
                {
                    if (!Group.Find("Nobody").commands.Contains("pay") && !Group.Find("Nobody").commands.Contains("give") && !Group.Find("Nobody").commands.Contains("take")) Player.SendMessage(p, "> > &a" + playerDb.Rows[0]["Money"] + Server.DefaultColor + " " + Server.Currency);
                }
                catch { }
                Player.SendMessage(p, "> > &cdied &a" + playerDb.Rows[0]["TotalDeaths"] + Server.DefaultColor + " times");
                Player.SendMessage(p, "> > &bmodified &a" + playerDb.Rows[0]["totalBlocks"] + " &eblocks.");
                Player.SendMessage(p, "> > was last seen on &a" + playerDb.Rows[0]["LastLogin"]);
                Player.SendMessage(p, "> > " + TotalTime(playerDb.Rows[0]["TimeSpent"].ToString()));
                Player.SendMessage(p, "> > first logged into the server on &a" + playerDb.Rows[0]["FirstLogin"]);
                Player.SendMessage(p, "> > logged in &a" + playerDb.Rows[0]["totalLogin"] + Server.DefaultColor + " times, &c" + playerDb.Rows[0]["totalKicked"] + Server.DefaultColor + " of which ended in a kick.");
                Player.SendMessage(p, "> > " + Awards.awardAmount(message) + " awards");
                if (Ban.Isbanned(message))
                {
                    string[] data = Ban.Getbandata(message);
                    Player.SendMessage(p, "> > was banned by " + data[0] + " for " + data[1] + " on " + data[2]);
                }

                if (Server.Devs.Contains(message.ToLower()))
                {
                    Player.SendMessage(p, Server.DefaultColor + "> > Player is a &9Developer");
                }

                if (!(p != null && (int)p.group.Permission <= CommandOtherPerms.GetPerm(this)))
                {
                    if (Server.bannedIP.Contains(playerDb.Rows[0]["IP"].ToString()))
                        playerDb.Rows[0]["IP"] = "&8" + playerDb.Rows[0]["IP"] + ", which is banned";
                    Player.SendMessage(p, "> > the IP of " + playerDb.Rows[0]["IP"]);
                    if (Server.useWhitelist)
                    {
                        if (Server.whiteList.Contains(message.ToLower()))
                        {
                            Player.SendMessage(p, "> > Player is &fWhitelisted");
                        }
                    }
                }
                playerDb.Dispose();
            }
            #endregion
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/info [player] - Provides information about [player].");
            Player.SendMessage(p, "/info ip [IP] - Provides information about the specified IP address.");
        }
        public string TotalTime(string time)
        {
            return "time spent on server: " + time.Split(' ')[0] + " Days, " + time.Split(' ')[1] + " Hours, " + time.Split(' ')[2] + " Minutes, " + time.Split(' ')[3] + " Seconds.";
        }
    }
}
