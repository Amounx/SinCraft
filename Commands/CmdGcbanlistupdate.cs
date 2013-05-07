/*
	Copyright 2011 MCForge (modified by Sinjai for use with SinCraft)
		
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.opensource.org/licenses/ecl2.php
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
namespace SinCraft.Commands
{
    public sealed class CmdGCBanListUpdate : Command
    {
        public override string name { get { return "gcbanlistupdate"; } }
        public override string[] aliases { get { return new string[] { "gcbu", "gcblu" }; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            Server.UpdateGlobalSettings();
            Player.GlobalMessage("The Global Banlist has been updated.");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/gcbanlistupdate - Updates the global chat ban list for this server.");
        }
    }
}