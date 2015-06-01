using System.Linq;
using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        private static ActorSystem _myActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            _myActorSystem = ActorSystem.Create("MyActorSystem");

            // time to make your first actors!
            var consoleWriteActor = _myActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()));
            var consoleReaderActor = _myActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriteActor)));

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            _myActorSystem.AwaitTermination();
        }
    }
    #endregion
}
