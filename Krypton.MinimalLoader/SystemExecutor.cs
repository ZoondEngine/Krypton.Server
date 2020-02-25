using Krypton.Support.CodeAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.MinimalLoader
{
    public class SystemExecutor
    {
        private Core.CodeExecutor.CodeExecuteComponent CodeEvaluator { get; set; }
        private Core.Hardware.HardwareComponent Hardware { get; set; }
        private Core.Http.HttpComponent Http { get; set; }
        private Core.Hyperthreading.ThreadingComponent Threads { get; set; }
        private Core.Network.NetworkComponent Network { get; set; }
        private Core.Prompt.PromptComponent Prompt { get; set; }
        private Core.Security.SecurityComponent Security { get; set; }
        private Core.StepSystem.StepComponent Stages { get; set; }

        public SystemExecutor()
        {
            if(Core.Security.SecurityComponent.Instance.IsDebug())
            {
                BuildDebug();
            }
            else
            {
                Build();
            }
        }

        public Core.CodeExecutor.CodeExecuteComponent GetCodeEvaluator()
            => CodeEvaluator;

        public Core.Hardware.HardwareComponent GetHardware()
            => Hardware;

        public Core.Http.HttpComponent GetHttp()
            => Http;

        public Core.Hyperthreading.ThreadingComponent GetThreading()
            => Threads;

        public Core.Network.NetworkComponent GetNetwork()
            => Network;

        public Core.Prompt.PromptComponent GetConsole()
            => Prompt;

        public Core.Security.SecurityComponent GetSecurity()
            => Security;

        public Core.StepSystem.StepComponent GetStageManager()
            => Stages;

        private void BuildDebug()
        {
            using (var analyze = Analyze.Watch("SystemExecutor"))
            {
                Build();
            }
        }
        private void Build()
        {
            CodeEvaluator = Core.CodeExecutor.CodeExecuteComponent.Instance;
            Hardware = Core.Hardware.HardwareComponent.Instance;
            Http = Core.Http.HttpComponent.Instance;
            Threads = Core.Hyperthreading.ThreadingComponent.Instance;
            Network = Core.Network.NetworkComponent.Instance;
            Prompt = Core.Prompt.PromptComponent.Instance;
            Security = Core.Security.SecurityComponent.Instance;
            Stages = Core.StepSystem.StepComponent.Instance;
        }

    }
}
