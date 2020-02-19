using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Client.Interfaces;

namespace ISTraining_Part.Client.Design
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
