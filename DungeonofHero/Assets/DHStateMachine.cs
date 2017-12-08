using UnityEngine;
using System;

    public struct DHStateChangeEvent<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        public GameObject Target;
        public DHStateMachine<T> TargetStateMachine;
        public T NewState;
        public T PreviousState;

        public DHStateChangeEvent(DHStateMachine<T> stateMachine)
        {
            Target = stateMachine.Target;
            TargetStateMachine = stateMachine;
            NewState = stateMachine.CurrentState;
            PreviousState = stateMachine.PreviousState;
        }
    }

    public interface MMIStateMachine
    {
        bool TriggerEvents { get; set; }
    }

    public class DHStateMachine<T> : MMIStateMachine where T : struct, IComparable, IConvertible, IFormattable
    {
        public bool TriggerEvents { get; set; }
        public GameObject Target;
        public T CurrentState { get; protected set; }
        public T PreviousState { get; protected set; }

        public DHStateMachine(GameObject target, bool triggerEvents)
        {
            this.Target = target;
            this.TriggerEvents = triggerEvents;
        }

        public virtual void ChangeState(T newState)
        {
            if (newState.Equals(CurrentState))
            {
                return;
            }

            PreviousState = CurrentState;
            CurrentState = newState;

            if (TriggerEvents)
            {
                MMEventManager.TriggerEvent(new DHStateChangeEvent<T>(this));
            }
        }

        public virtual void RestorePreviousState()
        {
            CurrentState = PreviousState;

            if (TriggerEvents)
            {
                MMEventManager.TriggerEvent(new DHStateChangeEvent<T>(this));
            }
        }
    }