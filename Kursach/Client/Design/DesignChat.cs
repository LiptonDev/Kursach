using Kursach.Client.Delegates;
using Kursach.Client.Interfaces;

namespace Kursach.Client.Design
{
    class DesignChat : IChat
    {
        public event OnChatMessage NewMessage;

        public void SendMessage(string text)
        {
            NewMessage?.Invoke("DESIGN NAME", text);
        }
    }
}
