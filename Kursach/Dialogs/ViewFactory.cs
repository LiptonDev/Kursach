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
        /// Конструктор.
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
                Logger.Log.Error($"DialogNameAttribute is null: {{{Logger.GetParamsNamesValues(() => viewModel)}}}");
                return null;
            }

            var view = container.Resolve<FrameworkElement>(atr.ViewName);

            if (view.GetType().Name == nameof(FrameworkElement))
            {
                Logger.Log.Error($"GetView<T> view is null: {{{Logger.GetParamsNamesValues(() => viewModel)}}}");
                return null;
            }

            view.DataContext = viewModel;

            return view;
        }
    }
}
