using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BandoriBot.Commands
{
    [Serializable]
    public class CommandException : Exception{
        public CommandException(string message) : base(message)
        {
        }

        public CommandException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CommandException()
        {
        }

        protected CommandException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
        }
    }
    public class LegacyCommand : Command
    {
        private string Command;
        private Action<CommandArgs> Action;

        public LegacyCommand(string Command, Action<CommandArgs> Action)
        {
            this.Command = Command;
            this.Action = Action;
        }

        protected override List<string> Alias => new List<string> { Command };

        protected override void Run(CommandArgs args)
        {
            try
            {
                Action(args);
            }
            
            catch (TargetInvocationException e)
            {
                if (e.InnerException is CommandException e2)
                    args.Callback(e2.Message);
                else
                    throw;
            }
            catch (CommandException e)
            {
                args.Callback(e.Message);
            }
        }
    }
}
