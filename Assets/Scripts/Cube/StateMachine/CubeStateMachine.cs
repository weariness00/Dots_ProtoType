using Unity.Entities;

namespace Game
{
    public class CubeStateMachine : IComponentData
    {
        public IState CurrentState = new IdleState();
        
        
    }
}