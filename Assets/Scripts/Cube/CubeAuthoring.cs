using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct SpawnCubeTag : IComponentData {}
    public struct CubeTag : IComponentData {}
    
    public struct CubeTagAuthoring : IComponentData
    {
        public float3 Position;
    }
}