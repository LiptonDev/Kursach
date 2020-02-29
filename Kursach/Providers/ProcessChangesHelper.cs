using ISTraining_Part.Core.ServerEvents;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ISTraining_Part.Providers
{
    static class ProcessChangesHelper
    {
        public static void ProcessChanges<T>(DbChangeStatus status, T arg, ObservableCollection<T> collection, TaskFactory sync)
        {
            switch (status)
            {
                case DbChangeStatus.Add:
                    if (!collection.Contains(arg))
                        sync.StartNew(() => collection.Add(arg));
                    break;

                case DbChangeStatus.Update:
                    int index = collection.IndexOf(arg);
                    if (index > -1)
                    {
                        sync.StartNew(() =>
                        {
                            collection.RemoveAt(index);
                            collection.Insert(index, arg);
                        });
                    }
                    break;

                case DbChangeStatus.Remove:
                    if (collection.Contains(arg))
                        sync.StartNew(() => collection.Remove(arg));
                    break;

                default:
                    Logger.Log.Warn($"Необработанное событие изменения: {{type: {typeof(T)}, arg: {arg}, status: {status}}}");
                    break;
            }
        }
    }
}
