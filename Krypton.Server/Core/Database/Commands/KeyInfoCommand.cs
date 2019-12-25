using Krypton.Server.Core.IO.Contracts;
using System.Linq;

namespace Krypton.Server.Core.Database.Commands
{
    public class KeyInfoCommand : ICommandElement
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }

        public KeyInfoCommand()
        {
            Name = "Keys helper";
            Description = "Command for getting keys information";
            Level = 1;
        }

        public string GetHelp()
            => ".db keys info";

        public bool IsHandlable(string line)
            => line.Contains(".db keys info");

        public bool Run(string line)
        {
            var splitted = line.Split(' ');
            using (var context = DatabaseMgr.Instance.GetKeysContext())
            {
                var print = IO.IOMgr.Instance.GetPrint();

                if (splitted.Length >3 && int.TryParse(splitted[3], out int key_id))
                {
                    var key = context.Keys.FirstOrDefault((x) => x.Id == key_id);
                    if(key != null)
                    {
                        print.Trace("--------------------------------------------------------");
                        print.Trace($" '{key.Id}' key information: ");
                        print.Trace("");
                        print.Warning($"   + DAYS: {key.Days} | Region: {key.RegionCode}");
                        print.Warning($"   + VALUE: {key.Value}");
                        print.Warning($"   + ACTIVATED_AT: {key.ActivatedAt}");
                        print.Warning($"   + END_AT:       {key.EndAt}");
                        print.Trace($"-------------------------------------------------------");
                    }
                    else
                    {
                        print.Error($"Key: '{key_id}' not found in database");
                    }
                }
                else
                {
                    if (splitted.Length > 3)
                    {
                        var key = context.Keys.FirstOrDefault((x) => x.Value == splitted[3]);
                        if (key != null)
                        {
                            print.Trace("--------------------------------------------------------");
                            print.Trace($" '{key.Id}' key information: ");
                            print.Trace("");
                            print.Warning($"   + DAYS: {key.Days} | Region: {key.RegionCode}");
                            print.Warning($"   + VALUE: {key.Value}");
                            print.Warning($"   + ACTIVATED_AT: {key.ActivatedAt}");
                            print.Warning($"   + END_AT:       {key.EndAt}");
                            print.Trace($"-------------------------------------------------------");
                        }
                    }
                    else
                    {
                        int all_keys = context.Keys.Count();
                        int day_keys = context.Keys.Where((x) => x.Days == 1).Count();
                        int week_keys = context.Keys.Where((x) => x.Days == 7).Count();
                        int month_keys = context.Keys.Where((x) => x.Days == 30).Count();
                        int active_keys = context.Keys.Where((x) => x.Hardware == null).Count();
                        int inactive_keys = all_keys - active_keys;

                        print.Trace("--------------------------------------------------------");
                        print.Trace(" Keys Information: ");
                        print.Trace("");
                        print.Warning($"   Keys in DB: {all_keys}");
                        print.Warning($"   1D: {day_keys} | 7D: {week_keys} | 30D: {month_keys}");
                        print.Warning($"   ACTIVE: {active_keys} | NOT ACTIVE: {inactive_keys} ");
                        print.Trace($"-------------------------------------------------------");
                    }

                }
            }

            return true;
        }
    }
}
