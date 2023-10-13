using UnityEngine;

namespace Game
{
    public class IdleState : State
    {
        private IdleState() { } 
        ~IdleState() { }
        
        public virtual void Enter() { Debug.Log("Enter IdleState");}
        public virtual void Execute() { Debug.Log("Execute IdleState");}
        public virtual void Exit() { Debug.Log("Exit IdleState");}
    }

}