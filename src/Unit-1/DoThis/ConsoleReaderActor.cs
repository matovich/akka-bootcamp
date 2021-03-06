using System;
using System.Runtime.Remoting.Messaging;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Shutdown"/>.
    /// </summary>
    class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        public const string StartCommand = "start";

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
                DoPrintInstructions();

            GetAndValidateInput();
        }

        private void DoPrintInstructions()
        {
            Console.WriteLine("Please provide the URI of a log file on disk.\n");
        }

        /// <summary>
        /// Reads input from console, validates it, then signals appropriate response
        /// (continue processing, error, success, etc.).
        /// </summary>
        private void GetAndValidateInput()
        {
            var message = Console.ReadLine();
            if (!string.IsNullOrEmpty(message) && String.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // if user typed ExitCommand, shut down the entire actor system (allows the process to exit)
                Context.System.Shutdown();
                return;
            }
            
            Context.ActorSelection("akka://MyActorSystem/user/fileValidatorActor").Tell(message);
        }

        /// <summary>
        /// Validates <see cref="message"/>.
        /// Currently says messages are valid if contain even number of characters.
        /// </summary>
        private static bool IsValid(string message)
        {
            return message.Length % 2 == 0;
        }
    }
}