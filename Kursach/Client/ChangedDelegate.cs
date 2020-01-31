using Kursach.Core.ServerEvents;

namespace Kursach.Client
{
    delegate void OnChanged<T>(DbChangeStatus status, T arg);
}
