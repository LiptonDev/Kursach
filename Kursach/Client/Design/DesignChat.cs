using Kursach.Client.Delegates;
using Kursach.Client.Interfaces;
using Kursach.Core.Models;

namespace Kursach.Client.Design
{
    class DesignChat : IChat
    {
        public event OnChatMessage NewMessage;

        public void SendMessage(string text)
        {
            NewMessage?.Invoke(new User { Name = "DESIGN LOGIN" }, text);
        }
    }
}
