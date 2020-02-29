using System;

namespace ISTraining_Part.Dialogs.Attributes
{
    /// <summary>
    /// Атрибут для диалоговых окон.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    class DialogNameAttribute : Attribute
    {
        /// <summary>
        /// Название View для диалога.
        /// </summary>
        public string ViewName { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DialogNameAttribute(string viewName)
        {
            ViewName = viewName;
        }
    }
}
