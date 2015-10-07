using System;
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
        private readonly IActorRef _validationActor;

        //private IActorRef _consoleWriterActor;

        public ConsoleReaderActor(IActorRef validationActor)
        {
            _validationActor = validationActor;
        }

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                DoPrintInstructions();
            }

            GetAndValidateInput();

            #region "Old Code"
            //var read = Console.ReadLine();
            //if (!string.IsNullOrEmpty(read) && String.Equals(read, ExitCommand, StringComparison.OrdinalIgnoreCase))
            //{
            //    // shut down the system (acquire handle to system via
            //    // this actors context)
            //    Context.System.Shutdown();
            //    return;
            //}

            //// send input to the console writer to process and print
            //// YOU NEED TO FILL IN HERE
            //_consoleWriterActor.Tell(read);

            //// continue reading messages from the console
            //// YOU NEED TO FILL IN HERE
            //Self.Tell("continue");
            #endregion
        }

        // in ConsoleReaderActor, after OnReceive()
        #region Internal methods
        private void DoPrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
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
            }

            _validationActor.Tell(message);

        }

        #endregion



    }
}