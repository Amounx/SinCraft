/********************************************************************************************************\
<+> Copyright 2013 Sinjai                                                                              
<+>                                                                                                    
<+> Licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported.         
<+> http://creativecommons.org/licenses/by-nc-sa/3.0/                                                 
<+>                                                                                                    
<+> This was written for use with SinCraft only.                                                   
<+> You must attribute the work in the manner specified by the author or licensor.                     
<+> You may not use this work for commercial purposes.                                                 
<+> If you alter, transform, or build upon this work, you may distribute the resulting work only under
<+> the same or similar license to this one.                                             
<+>                                                                                                    
<+> Any of the above conditions can be waived if you get written permission from the copyright holder.
\********************************************************************************************************/

namespace SinCraft.Commands
{
    public sealed class CmdSetName : Command
    {
        public override string name { get { return "setname"; } }
        public override string[] aliases { get { return new string[] { "nick", "nickname" }; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdSetName() { }
        public override void Use(Player p, string message)
        {
            string[] args = message.Split(' ');
            if (args[0].ToLower() == p.Username.ToLower())
            {
                if (args.Length == 1)
                {
                    p.SetName = p.Username;
                    p.save();
                    Player.GlobalMessage(p.color + p.Username + Server.DefaultColor + "'s name has been reset.");
                    return;
                }
                if (Player.CommandHasBadColourCodes(p, message) || args[1].Contains("'"))
                {
                    Player.SendMessage(p, "Invalid color codes in name!");
                    return;
                }
                p.SetName = args[1];
                p.save();
                Player.GlobalMessage(p.color + p.Username + Server.DefaultColor + " set his/her name to " + p.color + p.SetName);
            }
            else
            {
                if (p != null)
                    if (p.group.Permission < (LevelPermission)CommandOtherPerms.GetPerm(this) && !p.isDev)
                    {
                        Player.SendMessage(p, "You must be at least a(n) " + Group.findPermInt(CommandOtherPerms.GetPerm(Command.all.Find("setname"))).color + Group.findPermInt(CommandOtherPerms.GetPerm(Command.all.Find("setname"))).name + " to set other player's names.");
                        return;
                    }
                Player who = Player.Find(args[0]);
                if (who == null) { Player.SendMessage(p, "Specified player not found."); return; }
                if (args.Length == 1)
                {
                    who.SetName = who.Username;
                    who.save();
                    Player.GlobalMessage(who.color + who.Username + Server.DefaultColor + "'s name has been reset.");
                    return;
                }
                if (Player.HasBadColorCodes(args[1]) || Player.HasBadColorCodesTwo(args[1]))
                {
                    Player.SendMessage(p, "Invalid color codes in name!");
                    return;
                }
                who.SetName = args[1];
                who.save();
                Player.GlobalMessage(who.color + who.Username + Server.DefaultColor + "'s name was set to " + who.color + who.SetName);
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/setname [player] [name] - Set the display name of [player] to [name].");
            Player.SendMessage(p, "Leave [name] blank to reset the player's name.");
            Player.SendMessage(p, "You must be at least a(n) " + Group.findPermInt(CommandOtherPerms.GetPerm(Command.all.Find("setname"))).color + Group.findPermInt(CommandOtherPerms.GetPerm(Command.all.Find("setname"))).name + " to set other player's names.");
        }
    }
}
