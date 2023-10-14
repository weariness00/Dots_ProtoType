using Unity.Entities;
using UnityEngine;

namespace StateMachine
{
    public class CubeStateMachine : IComponentData
    {
        public IState CurrentState;
        public IState PreviousState;
        public IState GlobalState;
    }
}