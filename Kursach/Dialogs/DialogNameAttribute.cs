using System;

namespace Kursach.Dialogs
{
    class DialogNameAttribute : Attribute
    {
        public string ViewName { get; }

        public DialogNameAttribute(string viewName)
        {
            ViewName = viewName;
        }
    }
}
