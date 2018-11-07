using BasicClassLibrary.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTestApp.ViewModel
{
    public class MainWindowViewModel : NotifyDataErrorInfoDataAnnotations
    {
        #region Constructors
        static MainWindowViewModel()
        {
            propertyNames = new List<string>() { nameof(CurrentStatusBarViewModel),
                                                 nameof(Test) };
        }
        public MainWindowViewModel() : base()
        {
            ValidateAsync();
        }
        #endregion
        #region Variables and Properties
        private static List<string> propertyNames;
        /// <summary>
        /// Properties names that need to be validated
        /// </summary>
        protected override List<string> PropertyNames { get => propertyNames; set => propertyNames = value; }

        private string test;
        [Required(ErrorMessage = "The property {0} cannot be empty")]
        public string Test
        {
            get => test;
            set => SetProperty(ref test, value);
        }



        /// <summary>
        /// Current StatusBarViewModel
        /// </summary>
        [Required]
        public StatusBarViewModel CurrentStatusBarViewModel
        {
            get
            {
                return StatusBarViewModel.ViewModel;
            }
        }
        #endregion
    }
}
