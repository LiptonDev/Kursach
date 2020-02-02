using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Helpers
{
    /// <summary>
    /// Установка связи одной ObservableCollection с другой.
    /// </summary>
    static class ObservableCollectionRelationship
    {
        /// <summary>
        /// Установка связи одной ObservableCollection с другой.
        /// </summary>
        public static void SetRelationship<TSource, TRelation>(this ObservableCollection<TSource> sources,
                                                                    ObservableCollection<TRelation> relations,
                                                                    Func<TSource, TRelation, bool> predicate,
                                                                    TaskFactory sync)
        {
            sources = sources ?? throw new ArgumentNullException(nameof(sources));
            relations = relations ?? throw new ArgumentNullException(nameof(relations));
            predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            sync = sync ?? throw new ArgumentNullException(nameof(sync));

            sources.CollectionChanged += (sender, e) =>
            {
                if (e.Action != NotifyCollectionChangedAction.Remove)
                    return;

                TSource owner = (TSource)e.OldItems[0];
                var forRemove = relations.Where(x => predicate(owner, x));
                foreach (var item in forRemove)
                {
                    sync.StartNew(() => relations.Remove(item));
                }
            };
        }
    }
}
