using DryIoc;

namespace Server
{
    static class IocExt
    {
        public static void RegisterSingleton<TFrom, TTo>(this IContainer container) where TTo : TFrom
        {
            container.Register(typeof(TFrom), typeof(TTo), reuse: Reuse.Singleton);
        }

        public static void Register<TFrom, TTo>(this IContainer container) where TTo : TFrom
        {
            container.Register(typeof(TFrom), typeof(TTo));
        }
    }
}
