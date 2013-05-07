/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/SinCraft)
	
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
using System;
using System.Collections.Generic;
using System.Linq;
namespace SinCraft
{
    public sealed class CommandList
    {
        public List<Command> commands = new List<Command>();
        public CommandList() { }
        public void Add(Command cmd) { commands.Add(cmd); }
        public void AddRange(List<Command> listCommands)
        {
            listCommands.ForEach(delegate(Command cmd) { commands.Add(cmd); });
        }
        public List<string> commandNames()
        {
            var tempList = new List<string>();

            commands.ForEach(cmd => tempList.Add(cmd.name));

            return tempList;
        }

        public bool Remove(Command cmd) { return commands.Remove(cmd); }
        public bool Contains(Command cmd) { return commands.Contains(cmd); }
        public bool Contains(string name)
        {
            name = name.ToLower();
            return commands.Any(cmd => cmd.name == name.ToLower());
        }
        public Command Find(string name)
        {
            Command cmd = null;
            foreach (Command comm in Command.all.commands.ToArray())
            {
                if (comm.name == name)
                {
                    cmd = comm;
                    break;
                }
                foreach (string alias in comm.aliases)
                {
                    if (alias == name)
                    {
                        cmd = comm;
                        break;
                    }
                }
            }
            return cmd;
        }
        /// <summary>
        /// Used to find the command name that the alias is assigned to.
        /// </summary>
        /// <param name="alias">The alias to search for.</param>
        /// <returns></returns>
        public string FindCmdNameByAlias(string alias)
        {
            Command cmd = null;
            foreach (Command comm in Command.all.commands.ToArray())
            {
                foreach (string _alias in comm.aliases)
                {
                    if (_alias == alias)
                    {
                        cmd = comm;
                        break;
                    }
                }
            }
            return cmd.name;
            //return (cmd != null) ? gotten.name : "";
        }
        public List<Command> All() { return new List<Command>(commands); }
    }
}