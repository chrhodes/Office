using Prism.Events;

namespace SupportTools_Visio.Infrastructure
{
    public class SelectionChangedEvent : PubSubEvent { }
    public class LoadPageEvent : PubSubEvent { }
    public class SavePageEvent : PubSubEvent { }
}
