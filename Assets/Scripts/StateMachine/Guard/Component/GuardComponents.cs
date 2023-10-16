using Unity.Entities;
using Unity.Mathematics;

namespace StateMachine
{
    [InternalBufferCapacity(8)]
    public struct WaypointPosition : IBufferElementData
    {
        public float3 Value;
    }

    public struct TargetPosition : IComponentData
    {
        public float3 Value;
    }
}