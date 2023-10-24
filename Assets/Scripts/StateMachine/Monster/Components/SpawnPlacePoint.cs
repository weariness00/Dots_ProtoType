using Unity.Entities;
using Unity.Mathematics;

namespace StateMachine.Monster
{
    [InternalBufferCapacity(5)]
    public struct SpawnPlacePoint : IBufferElementData
    {
        public float3 Value;
    }
}