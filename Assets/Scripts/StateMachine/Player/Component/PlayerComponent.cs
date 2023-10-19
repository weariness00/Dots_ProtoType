using Unity.Entities;
using Unity.Mathematics;

namespace StateMachine
{
    public struct PlayerIdleStateTag : IComponentData {}
    public struct PlayerRunStateTag : IComponentData {}
    
    public struct UserInputData : IComponentData
    {
        public float2 Move;
    }

    public struct MovementSpeed : IComponentData
    {
        public float MeterPerSecond;
    }
}