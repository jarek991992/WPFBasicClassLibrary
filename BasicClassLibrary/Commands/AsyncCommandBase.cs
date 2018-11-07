using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BasicClassLibrary.Commands
{
    public abstract class AsyncCommandBase : IAsyncCommand
    {
        #region Variables and Properties
        public bool IsExecuting { get; private set; }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion

        public AsyncCommandBase()
        {
            IsExecuting = false;
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public virtual bool CanExecute( object parameter )
        {
            return !IsExecuting;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public virtual async void Execute( object parameter )
        {
            IsExecuting = true;
            try
            {
                await ExecuteAsync( parameter );
            }
            finally
            {
                IsExecuting = false;
                OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public virtual Task ExecuteAsync( object parameter )
        {
            if ( CanExecute( parameter ) )
                return OnExecute( parameter );
            else
                return null;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected abstract Task OnExecute( object parameter );
    }
}
