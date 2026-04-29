using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Script.ScriptableObject.Event
{
    public class GameEvent
    {
        
    }
    [CreateAssetMenu(fileName = "new EventChannel", menuName = "Event/EventChannel", order = 0)]
    public class EventChannelSO : UnityEngine.ScriptableObject
    {
        private Dictionary<Type, Action<GameEvent>> _events = new();
        private Dictionary<Delegate, Action<GameEvent>> _lookup = new();

        public void AddListener<T>(Action<T> handler) where T : GameEvent
        {
            if (_lookup.ContainsKey(handler)) return;
            
            //Action<T>를 Action<GameEvent>로 명시적 형변환을 통해 래핑
            Action<GameEvent> wrappedHandler = e => handler((T)e);

            //나중에 handler를 통해 wrappedHandle를 구독해제하기 위해 미리 매핑해놓기
            _lookup[handler] = wrappedHandler;
            
            //아무도 구독을 안했다면 래핑된 Action이 화자이자 청자가 되고, 이미 화자가 있다면 청자가 된다.
            Type evtType = typeof(T);
            if (!_events.TryAdd(evtType, wrappedHandler))
            {
                _events[evtType] += wrappedHandler;
            }
        }

        public void RemoveListener<T>(Action<T> handler) where T : GameEvent
        {
            Type evtType = typeof(T);
            if (!_lookup.TryGetValue(handler, out Action<GameEvent> wrappedHandler)) return;

            if (_events.TryGetValue(evtType, out Action<GameEvent> evtHandler))
            {
                evtHandler -= wrappedHandler;
                //만약 래핑된 Action이 화자가 된 후 아무도 연이어 구독하지 않았다면 화자가 청자와 같은 객체가 된다.
                //즉 evtHandler와 wrappedHandler가 같은 객체가 되는 것이기 때문에 구독 해제하면 null이 된다.
                if (evtHandler == null)
                    _events.Remove(evtType);
                else
                    _events[evtType] = evtHandler;
            }
            _lookup.Remove(handler);
        }

        public void RaiseEvent(GameEvent evt)
        {
            if(_events.TryGetValue(evt.GetType(), out Action<GameEvent> evtHandler))
                evtHandler?.Invoke(evt);
        }

        public void Clear()
        {
            _events.Clear();
            _lookup.Clear();
        }
    }
}