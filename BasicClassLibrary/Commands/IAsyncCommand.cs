using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BasicClassLibrary.Commands
{
    /// <summary>
    /// An async version of <see cref="ICommand"/>.
    /// </summary>
    interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// Executes the asynchronous command.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        Task ExecuteAsync( object parameter );
    }
}
