using DryIoc;
using System;
using System.Windows;

namespace Kursach.Dialogs
{
    /// <summary>
    /// View factory.
    /// </summary>
    class ViewFactory : IViewFactory
    {
        /// <summary>
        /// Контейнер.
        /// </summary>
        readonly IContainer container;

        /// <summary>
        /// Ctor.
        /// </summary>
        public ViewFactory(IContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Получить view by viewmodel.
        /// </summary>
        /// <returns></returns>
        public object GetView<TViewModel>(TViewModel viewModel)
        {
            var atr = (DialogNameAttribute)Attribute.GetCustomAttribute(typeof(TViewModel), typeof(DialogNameAttribute));

            if (atr == null)
            {
                Logger.Log.Error($"DialogNameAttribute is null, ViewModel: {viewModel}");
                return null;
            }

            var view = container.Resolve<FrameworkElement>(atr.ViewName);
            view.DataContext = viewModel;

            return view;
        }
    }
}
