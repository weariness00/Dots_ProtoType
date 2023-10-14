using Unity.Entities;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine : ICleanupComponentData
    {
        public IState CurrentState;
        public IState PreviousState;
        public IState GlobalState;
    }
}