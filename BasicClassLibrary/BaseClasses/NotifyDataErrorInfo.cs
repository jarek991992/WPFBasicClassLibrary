using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BasicClassLibrary.BaseClasses
{
    /// <summary>
    /// Provides functionality to provide errors for the object if it is in an invalid state. NET 4.5 or above
    /// </summary>
    /// <typeparam name="T">The type of this instance.</typeparam>
    public abstract class NotifyDataErrorInfoDelegateRule<T> : DataErrorInfoBaseClass, INotifyDataErrorInfo where T : NotifyDataErrorInfoDelegateRule<T> // where T it means that T class need to inherit this class 
    {
        private static List<DelegateRule<T>> rules;

        #region Properties and events
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        /// <summary>
        /// Gets the rules which provide the errors.
        /// </summary>
        /// <value>The rules this instance must satisfy.</value>
        protected static List<DelegateRule<T>> Rules
        {
            get { return rules; }
        }

        /// <summary>
        /// Gets a value indicating whether the object has validation errors. 
        /// </summary>
        /// <value><c>true</c> if this instance has errors, otherwise <c>false</c>.</value>
        public virtual bool HasErrors
        {
            get
            {
                return errorMessages.Any(kv => kv.Value != null && kv.Value.Count > 0);
            }
        }
        #endregion

        #region Constructors
        static NotifyDataErrorInfoDelegateRule()
        {
            rules = new List<DelegateRule<T>>();
        }

        public NotifyDataErrorInfoDelegateRule() : base()
        {
            ApplyRules();
        }
        #endregion

        #region Protected Methods
        /// <summary>
		/// Notifies listeners that a property value has changed.
		/// </summary>
		/// <param name="propertyName">Name of the property used to notify listeners. This
		/// value is optional and can be provided automatically when invoked from compilers
		/// that support <see cref="CallerMemberNameAttribute"/>.</param>
		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (string.IsNullOrEmpty(propertyName)) ApplyRules();
            else ApplyRules(propertyName);

            base.OnPropertyChanged(nameof(HasErrors));
        }

        /// <summary>
		/// Notifies listeners that a errors has changed.
		/// </summary>
		/// <param name="propertyName">Name of the property used to notify listeners. This
		/// value is optional and can be provided automatically when invoked from compilers
		/// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = null)
        {
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
		/// Raises this object's ErrorsChanged event.
		/// </summary>
		/// <param name="args">The DataErrorsChangedEventArgs</param>
		protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs args)
        {
            ErrorsChanged?.Invoke(this, args);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Applies all rules to this instance.
        /// </summary>
        private void ApplyRules()
        {
            lock (_classlock)
            {
                foreach (string propertyName in rules.Select(x => x.PropertyName))
                {
                    this.ApplyRules(propertyName);
                }
            }
        }
        /// <summary>
        /// Applies the rules to this instance for the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void ApplyRules(string propertyName)
        {
            List<string> propertyErrors = Apply((T)this, propertyName);

            if (propertyErrors.Count > 0)
            {
                if (errorMessages.ContainsKey(propertyName))
                {
                    errorMessages[propertyName].Clear();
                }
                else
                {
                    errorMessages[propertyName] = new List<string>();
                }

                errorMessages[propertyName].AddRange(propertyErrors);
                OnErrorsChanged(propertyName);
            }
            else if (errorMessages.ContainsKey(propertyName))
            {
                errorMessages[propertyName].Clear();
                OnErrorsChanged(propertyName);
            }
        }

        /// <summary>
        /// Applies the <see cref="Rule{T}"/>'s contained in this instance to <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to apply the rules to.</param>
        /// <param name="propertyName">Name of the property we want to apply rules for. <c>null</c>
        /// to apply all rules.</param>
        /// <returns>A collection of errors.</returns>
        private List<string> Apply(T obj, string propertyName)
        {
            List<string> errors = new List<string>();

            foreach (Rule<T> rule in rules)
            {
                if (string.IsNullOrEmpty(propertyName) || rule.PropertyName.Equals(propertyName))
                {
                    if (!rule.Apply(obj))
                    {
                        errors.Add(rule.Error);
                    }
                }
            }

            return errors;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the validation errors for a specified property or for the entire object.
        /// </summary>
        /// <param name="propertyName">Name of the property to retrieve errors for. <c>null</c> to 
        /// retrieve all errors for this instance.</param>
        /// <returns>A collection of errors.</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            List<string> result;
            if (string.IsNullOrEmpty(propertyName))
            {
                result = new List<string>();

                foreach (var errorMessages in this.errorMessages.Values)
                {
                    result.AddRange(errorMessages);
                }
            }
            else
            {
                errorMessages.TryGetValue(propertyName, out result);
            }

            return result;
        }

        public Task ApplyRulesAsync()
        {
            return Task.Run(() => ApplyRules());
        }
        #endregion
    }

    /// <summary>
    /// Provides functionality to provide errors for the object if it is in an invalid state. NET 4.5 or above
    /// </summary>
    /// <typeparam name="T">The type of this instance.</typeparam>
    public abstract class NotifyDataErrorInfoDataAnnotations : DataErrorInfoBaseClass, INotifyDataErrorInfo
    {
        #region Properties and events
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Gets a value indicating whether the object has validation errors. 
        /// </summary>
        /// <value><c>true</c> if this instance has errors, otherwise <c>false</c>.</value>
        public virtual bool HasErrors
        {
            get
            {
                return errorMessages.Any(kv => kv.Value != null && kv.Value.Count > 0);
            }
        }
        #endregion

        #region Constructors
        // Not needed 
        #endregion

        #region Protected Methods
        /// <summary>
		/// Notifies listeners that a property value has changed.
		/// </summary>
		/// <param name="propertyName">Name of the property used to notify listeners. This
		/// value is optional and can be provided automatically when invoked from compilers
		/// that support <see cref="CallerMemberNameAttribute"/>.</param>
		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            ValidateAsync(propertyName);
        }

        /// <summary>
        /// Notifies listeners that a errors has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = null)
        {
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
		/// Raises this object's ErrorsChanged event.
		/// </summary>
		/// <param name="args">The DataErrorsChangedEventArgs</param>
		protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs args)
        {
            ErrorsChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Check validation of property.
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <returns></returns>
        protected virtual List<string> OnValidate(string propertyName = null)
        {
            List<string> resultList;
            ValidationContext context = new ValidationContext(this)
            {
                MemberName = propertyName
            };

            Collection<ValidationResult> validationResults = new Collection<ValidationResult>();
            bool isValid = Validator.TryValidateObject(this, context, validationResults, true);
            if (!isValid)
            {
                foreach (var key in errorMessages.Keys)
                {
                    if (validationResults.All(result => result.MemberNames.All(memberName => !memberName.Equals(key))))
                    {
                        errorMessages[key].Clear();
                        OnErrorsChanged(key);
                    }
                    else
                    {
                        errorMessages[key].Clear();
                        var errors = validationResults.Where(result => result.MemberNames.Any(memberName => memberName.Equals(key))).Select(x => x.ErrorMessage);
                        errorMessages[key].AddRange(errors);
                        OnErrorsChanged(key);
                    }
                }
            }
            else
            {
                foreach (var key in errorMessages.Keys)
                {
                    errorMessages[key].Clear();
                    OnErrorsChanged(key);
                }
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                resultList = new List<string>();
                foreach (var errorMessages in this.errorMessages.Values)
                {
                    resultList.AddRange(errorMessages);
                }
            }
            else
            {
                errorMessages.TryGetValue(propertyName, out resultList);
            }

            return resultList;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the validation errors for a specified property or for the entire object.
        /// </summary>
        /// <param name="propertyName">Name of the property to retrieve errors for. <c>null</c> to 
        /// retrieve all errors for this instance.</param>
        /// <returns>A collection of errors.</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            errorMessages.TryGetValue(propertyName, out List<string> errorsForName);
            return errorsForName;
        }

        /// <summary>
        /// Check validation of properties async.
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <returns></returns>
        public Task ValidateAsync(string propertyName = null)
        {
            return Task.Run(() =>
            {
                lock (_classlock)
                {
                    return OnValidate(propertyName);
                }
            });
        }
        #endregion
    }
}
