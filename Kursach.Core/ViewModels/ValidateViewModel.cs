using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ISTraining_Part.Core.ViewModels
{
    /// <summary>
    /// ViewModel для проверки полей на ошибки.
    /// </summary>
    public abstract class ValidateViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Получение значения св-ва по имени.
        /// </summary>
        /// <param name="propName">Имя св-ва.</param>
        /// <returns></returns>
        private object GetPropertyValue(string propName) => GetType().GetProperty(propName).GetValue(this);

        /// <summary>
        /// Ошибки в проверке полей.
        /// </summary>
        [Write(false)]
        [JsonIgnore]
        [ChangeIgnore]
        public string Error => string.Join(Environment.NewLine, GetValidationErrors());

        /// <summary>
        /// Наличие ошибок.
        /// </summary>
        [Write(false)]
        [JsonIgnore]
        [ChangeIgnore]
        public bool IsValid => !GetValidationErrors().Any();

        /// <summary>
        /// Получение ошибки по имени св-ва.
        /// </summary>
        /// <param name="propName">Имя св-ва.</param>
        /// <returns></returns>
        [Write(false)]
        [JsonIgnore]
        [ChangeIgnore]
        public string this[string propName] => GetValidationError(propName);

        /// <summary>
        /// Получение ошибки по имени св-ва.
        /// </summary>
        /// <param name="propertyName">Имя св-ва.</param>
        /// <returns></returns>
        protected string GetValidationError(string propertyName)
        {
            string error = string.Empty;
            var context = new ValidationContext(this) { MemberName = propertyName };
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateProperty(GetPropertyValue(propertyName), context, results))
            {
                error = results.First().ErrorMessage;
            }

            return error;
        }

        /// <summary>
        /// Получение всех ошибок модели.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<ValidationResult> GetValidationErrors()
        {
            var context = new ValidationContext(this);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(this, context, results, true);

            return results;
        }
    }
}
