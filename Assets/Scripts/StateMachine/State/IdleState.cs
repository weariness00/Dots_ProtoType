using UnityEngine;

namespace StateMachine
{
    public class IdleState : IState
    {
        public void Enter() { Debug.Log("Enter IdleState");}
        public void Execute() { Debug.Log("Execute IdleState");}
        public void Exit() { Debug.Log("Exit IdleState");}
    }
}