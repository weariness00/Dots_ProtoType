using Unity.Entities;
using Unity.Mathematics;

namespace StateMachine
{
    public struct UserInputData : IComponentData
    {
        public float2 Move;
    }

    public struct MovementSpeed : IComponentData
    {
        public float MeterPerSecond;
    }
}