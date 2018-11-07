using BasicClassLibrary.BaseClasses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace WPFTestApp.ViewModel
{
    public class StatusBarViewModel : NotifyDataErrorInfoDataAnnotations
    {
        static StatusBarViewModel()
        {
            propertyNames = new List<string> { nameof(ProjectNameAndVersion) };
        }

        private StatusBarViewModel() : base() 
        {
            ValidateAsync();
        }
        #region Properties and events
        private static object classLock = typeof(StatusBarViewModel);
        private static StatusBarViewModel singelton;
        public static StatusBarViewModel ViewModel
        {
            get
            {
                lock (classLock)
                {
                    if (singelton == null)
                    {
                        singelton = new StatusBarViewModel();
                    }
                    return singelton;
                }
            }
        }

        private static List<string> propertyNames;
        /// <summary>
        /// Properties names that need to be validated
        /// </summary>
        protected override List<string> PropertyNames { get => propertyNames; set => propertyNames = value; }

        /// <summary>
        /// Current application name and version
        /// </summary>
        [Required(ErrorMessage = "The property {0} cannot be empty")]
        public static string ProjectNameAndVersion
        {
            get
            {
                FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                return fileInfo.ProductName + ": " + fileInfo.ProductVersion;
            }
        }

        private static string message;
        /// <summary>
        /// Current application status message
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                SetProperty(ref message, value as string);
            }
        }

        #endregion
    }
}
