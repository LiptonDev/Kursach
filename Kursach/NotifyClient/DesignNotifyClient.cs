using System;
using System.Threading.Tasks;

namespace Kursach.NotifyClient
{
    class DesignNotifyClient : INotifyClient
    {
        public event Action Connected;
        public event Action Disconnected;
        public event Action Reconnected;
        public event Notify StudentChanged;
        public event Notify GroupChanged;
        public event Action StaffChanged;
        public event Action UserChanged;

        /// <summary>
        /// Отключиться от сервера.
        /// </summary>
        public void Disconnect()
        {
        }

        public void ChangeGroup(int oldDivision, int newDivision)
        {
            GroupChanged?.Invoke(oldDivision, newDivision);
        }

        public void ChangeStaff()
        {
            StaffChanged?.Invoke();
        }

        public void ChangeStudent(int oldGroupId, int newGroupId)
        {
            StudentChanged?.Invoke(oldGroupId, newGroupId);
        }

        public void ChangeUser()
        {
            UserChanged?.Invoke();
        }

        public Task ConnectAsync()
        {
            Connected?.Invoke();
            return Task.CompletedTask;
        }

        public void SetStatus(bool status)
        {
        }
    }
}
