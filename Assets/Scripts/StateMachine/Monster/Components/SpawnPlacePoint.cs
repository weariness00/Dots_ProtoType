using Unity.Entities;
using Unity.Mathematics;

namespace StateMachine.Monster
{
    public struct SpawnPlacePoint : IBufferElementData
    {
        public float3 Value;
    }
}