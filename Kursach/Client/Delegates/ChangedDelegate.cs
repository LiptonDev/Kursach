using Kursach.Core.ServerEvents;

namespace Kursach.Client.Delegates
{
    delegate void OnChanged<T>(DbChangeStatus status, T arg);
}
