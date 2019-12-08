namespace Kursach.Dialogs
{
    /// <summary>
    /// View factory.
    /// </summary>
    interface IViewFactory
    {
        /// <summary>
        /// Получить view.
        /// </summary>
        /// <returns></returns>
        object GetView<TViewModel>(TViewModel viewModel);
    }
}
