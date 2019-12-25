using Krypton.Server.Core.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.Database.Commands
{
    public class KeyPacketsInfoCommand : ICommandElement
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }

        public KeyPacketsInfoCommand()
        {
            Name = "Key packets helper";
            Description = "Command for getting key packets information";
            Level = 1;
        }

        public string GetHelp()
            => ".db key packets info";

        public bool IsHandlable(string line)
            => line.Contains(".db key packets info");

        public bool Run(string line)
        {
            using(var context = DatabaseMgr.Instance.GetKeysContext())
            {
                foreach(var packet in context.KeyPackets)
                {
                    IO.IOMgr.Instance.GetPrint().Trace($"Packet: {packet.Name}[{packet.Identifier}], OwnerID: {packet.OwnerId}, Stamp: {packet.GeneratedStamp}, Keys: {packet.GetKeys().Count}");
                }
            }

            return true;
        }
    }
}
