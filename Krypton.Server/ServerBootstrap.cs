using Krypton.Server.Core.IO;
using Krypton.Support;
using Krypton.Support.CodeAnalyzer;
using Krypton.Support.ComponentModel.Contracts;
using System;
using System.Linq;
using System.Reflection;

namespace Krypton.Server
{
    public class ServerBootstrap : KryptonComponent<ServerBootstrap>
    {

        public ServerBootstrap()
        {
            
        }

        public void Boot()
        {
            CallAll("OnServerInitialized");
        }

        public void Stop()
        {
            CallAll("OnServerShutdown");
        }

        private void CallAll(string hook_name, bool injecting = true)
        {
            using (Analyze.Watch("Server initialization"))
            {
                var components = Assembly
                       .GetExecutingAssembly()
                       .GetTypes()
                       .Where(m => m.GetInterfaces().Contains(typeof(IKryptonComponent)))
                       .Select(m => m.GetConstructor(Type.EmptyTypes).Invoke(null) as IKryptonComponent)
                       .ToList();

                for (var i = 0; i < components.Count; i++)
                {
                    var method = components[i].GetType().GetMethod(hook_name);
                    if (injecting & method != null)
                    {
                        var parameters = method.GetParameters();
                        var required = new object[parameters.Length];

                        for (var j = 0; j < parameters.Length; j++)
                        {
                            for (var k = 0; k < components.Count; k++)
                            {
                                if (components[k].GetType() == parameters[j].ParameterType)
                                {
                                    required[j] = components[k];
                                }
                            }
                        }

                        method?.Invoke(components[i], required);
                    }
                    else
                    {
                        method?.Invoke(components[i], null);
                    }
                }
            }

            //Must be after all components
            GetComponent<IOMgr>().RunPrompt();
        }
    }
}
