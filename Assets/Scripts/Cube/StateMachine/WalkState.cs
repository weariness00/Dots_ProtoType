using UnityEngine;

namespace Game
{
    public class WalkState : IState
    {
        public void Enter() { Debug.Log("Enter WalkState");}
        public void Execute() { Debug.Log("Execute WalkState");}
        public void Exit() { Debug.Log("Exit WalkState");}
    }
}