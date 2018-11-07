using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicClassLibrary.Commands
{
    public class DelegateAsyncCommand : AsyncCommandBase
    {
        /// <summary>
        /// Token to cancel async command.
        /// </summary>
        private CancellationTokenSource cancelToken;

        /// <summary>
        /// Can execute delegate.
        /// </summary>
        public Predicate<object> CanExecuteDelegate { get; private set; }

        /// <summary>
        /// Execute delegate.
        /// </summary>
        public Func<object, CancellationToken, Task> ExecuteDelegate { get; private set; }

        /// <summary>
        /// Cancel command.
        /// </summary>
        public DelegateCommand CancelCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateAsyncCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public DelegateAsyncCommand( Func<object, CancellationToken, Task> execute, Predicate<object> canExecute = null ) : base()
        {
            this.CanExecuteDelegate = canExecute;
            this.ExecuteDelegate = execute ?? throw new ArgumentNullException( "Execute" );
            cancelToken = new CancellationTokenSource();
            CancelCommand = new DelegateCommand( (o) => cancelToken?.Cancel(), (o) => true );
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed(default if CanExecuteDelegate not set); otherwise, false.
        /// </returns>
        public override bool CanExecute( object parameter )
        {
            return ( CanExecuteDelegate != null ) ? ( !IsExecuting && CanExecuteDelegate( parameter ) ) : !IsExecuting;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        protected override Task OnExecute( object parameter )
        {
            return ExecuteDelegate( parameter, cancelToken.Token );
        }
    }
}
