using System;

namespace BotFactory.Models
{
    public class StatusChangedEventArgs : EventArgs
    {
        public string NewStatus { get; set; }

    }
}