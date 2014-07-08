using PS.Events;

namespace PS.Core
{
    /// <summary>
    /// Delegate for events.
    /// </summary>
    /// <typeparam name="T">Type of event.</typeparam>
    /// <param name="message">Message to send.</param>
    public delegate void EventDelegate<T>(T message);
}
