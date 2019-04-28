using System.Collections.Generic;

public enum EventId
{
    VRControllerGripPress,
    VRControllerGripRelease,
    VRControllerTriggerPress,
    VRControllerTriggerRelease,
    VRControllerTouchpadPress,
    VRControllerTouchpadDrag,
    VRControllerTouchpadRelease,
    ManipulatorEnter,
    ManipulatorExit,
    ObjectGrabbed,
    ObjectReleased,
    RecordingStarted,
    RecordingEnded
}

public delegate bool EventCallback(object cookie);

public class EventManager
{
    private Dictionary<EventId, List<EventCallback>> callbacks;

	public void AddListener(EventId evt, EventCallback callback)
    {
        if (callbacks == null)
        {
            callbacks = new Dictionary<EventId, List<EventCallback>>();
        }

        if (!callbacks.ContainsKey(evt))
        {
            callbacks.Add(evt, new List<EventCallback>());
        }

        callbacks[evt].Add(callback);
    }

    public void RemoveListener(EventId evt, EventCallback callback)
    {
        if (callbacks != null && callbacks.ContainsKey(evt) && callbacks[evt].Contains(callback))
        {
            callbacks[evt].Remove(callback);
        }
    }

    public void SendEvent(EventId evt, object cookie)
    {
        if (callbacks != null && callbacks.ContainsKey(evt))
        {
            for (int i = 0, count = callbacks[evt].Count; i < count; ++i)
            {
                bool eventEaten = callbacks[evt][i](cookie);
                if (eventEaten)
                {
                    break;
                }
            }
        }
    }

    public void Destroy()
    {
        callbacks = null;
    }
}
