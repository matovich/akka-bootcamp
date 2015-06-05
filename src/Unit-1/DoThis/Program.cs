using System.Linq;
using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        private static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            // time to make your first actors!
            //var consoleWriteActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()));
            //var consoleReaderActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriteActor)));

            var tailCoordinatorActor = MyActorSystem.ActorOf(Props.Create(() => new TailCoordinatorActor()), "tailCoordinatorActor");
            var consoleWriterActor = MyActorSystem.ActorOf(Props.Create<ConsoleWriterActor>(), "consoleWriterActor");
            var fileValidatorActor = MyActorSystem.ActorOf(Props.Create(() => new FileValidatorActor(consoleWriterActor, tailCoordinatorActor)), "fileValidatorActor");
            var consoleReaderActor = MyActorSystem.ActorOf(Props.Create<ConsoleReaderActor>(fileValidatorActor), "consoleReaderActor");

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.AwaitTermination();
        }
    }
    #endregion
}
