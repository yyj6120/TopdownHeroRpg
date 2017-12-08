//#define EVENTROUTER_THROWEXCEPTIONS 
#if EVENTROUTER_THROWEXCEPTIONS
//#define EVENTROUTER_REQUIRELISTENER // Uncomment this if you want listeners to be required for sending events.
#endif

using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public struct DHGameEvent
{
    public string EventName;
    public DHGameEvent(string newName)
    {
        EventName = newName;
    }
}

public struct DHSfxEvent
{
    public AudioClip ClipToPlay;
    public DHSfxEvent(AudioClip clipToPlay)
    {
        ClipToPlay = clipToPlay;
    }
}

[ExecuteInEditMode]
public static class MMEventManager
{
    private static Dictionary<Type, List<DHEventListenerBase>> _subscribersList;

    static MMEventManager()
    {
        _subscribersList = new Dictionary<Type, List<DHEventListenerBase>>();
    }

    public static void AddListener<MMEvent>(DHEventListener<MMEvent> listener) where MMEvent : struct
    {
        Type eventType = typeof(MMEvent);

        if (!_subscribersList.ContainsKey(eventType))
            _subscribersList[eventType] = new List<DHEventListenerBase>();

        if (!SubscriptionExists(eventType, listener))
            _subscribersList[eventType].Add(listener);
    }

    public static void RemoveListener<MMEvent>(DHEventListener<MMEvent> listener) where MMEvent : struct
    {
        Type eventType = typeof(MMEvent);

        if (!_subscribersList.ContainsKey(eventType))
        {
#if EVENTROUTER_THROWEXCEPTIONS
					throw new ArgumentException( string.Format( "Removing listener \"{0}\", but the event type \"{1}\" isn't registered.", listener, eventType.ToString() ) );
#else
            return;
#endif
        }

        List<DHEventListenerBase> subscriberList = _subscribersList[eventType];
        bool listenerFound;
        listenerFound = false;

        if (listenerFound)
        {

        }

        for (int i = 0; i < subscriberList.Count; i++)
        {
            if (subscriberList[i] == listener)
            {
                subscriberList.Remove(subscriberList[i]);
                listenerFound = true;

                if (subscriberList.Count == 0)
                    _subscribersList.Remove(eventType);

                return;
            }
        }

#if EVENTROUTER_THROWEXCEPTIONS
		        if( !listenerFound )
		        {
					throw new ArgumentException( string.Format( "Removing listener, but the supplied receiver isn't subscribed to event type \"{0}\".", eventType.ToString() ) );
		        }
#endif
    }

    public static void TriggerEvent<MMEvent>(MMEvent newEvent) where MMEvent : struct
    {
        List<DHEventListenerBase> list;
        if (!_subscribersList.TryGetValue(typeof(MMEvent), out list))
#if EVENTROUTER_REQUIRELISTENER
			            throw new ArgumentException( string.Format( "Attempting to send event of type \"{0}\", but no listener for this type has been found. Make sure this.Subscribe<{0}>(EventRouter) has been called, or that all listeners to this event haven't been unsubscribed.", typeof( MMEvent ).ToString() ) );
#else
            return;
#endif

        for (int i = 0; i < list.Count; i++)
        {
            (list[i] as DHEventListener<MMEvent>).OnMMEvent(newEvent);
        }
    }

    private static bool SubscriptionExists(Type type, DHEventListenerBase receiver)
    {
        List<DHEventListenerBase> receivers;

        if (!_subscribersList.TryGetValue(type, out receivers)) return false;

        bool exists = false;

        for (int i = 0; i < receivers.Count; i++)
        {
            if (receivers[i] == receiver)
            {
                exists = true;
                break;
            }
        }

        return exists;
    }
}

public static class EventRegister
{
    public delegate void Delegate<T>(T eventType);

    public static void MMEventStartListening<EventType>(this DHEventListener<EventType> caller) where EventType : struct
    {
        MMEventManager.AddListener<EventType>(caller);
    }

    public static void MMEventStopListening<EventType>(this DHEventListener<EventType> caller) where EventType : struct
    {
        MMEventManager.RemoveListener<EventType>(caller);
    }
}

public interface DHEventListenerBase { };

public interface DHEventListener<T> : DHEventListenerBase
{
    void OnMMEvent(T eventType);
}