using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Script.Agent.FSM
{
    public class AgentStateMachine
    {
        public AgentState CurrentState { get; private set; }

        private Dictionary<int, AgentState> _stateDict;

        public AgentStateMachine(Agent agent, StateSO[] stateList)
        {
            _stateDict = new Dictionary<int, AgentState>();
            
            foreach (StateSO state in stateList)
            {
                Type stateType = Type.GetType(state.className);
                Debug.Assert(stateType != null, $"State is Null! {state.className}");
                
                AgentState agentState = Activator.CreateInstance(stateType, agent, state.stateParam) as AgentState;
                
                _stateDict.Add(state.stateIndex, agentState);
            }
        }

        public void ChangeState(int nextStateIndex)
        {
            CurrentState?.Exit();

            AgentState nextState = _stateDict.GetValueOrDefault(nextStateIndex);
            Debug.Assert(nextState != null, $"State is Null! {nextStateIndex}");
            
            CurrentState = nextState;
            CurrentState?.Enter();
        }

        public void UpdateStateMachine() => CurrentState?.Update();
    }
}