using Unity.Entities;
using Unity.Mathematics;

namespace Script.Spawner
{
    public struct SpawnerTag : IComponentData{}
    
    public struct SpawnerAuthoring : IComponentData
    {
        public int SpawnIndex;
        public float SpawnInterval;

        public float CurrentTimer;
        public int SpawnNumber;
    }

    public struct SpawnerEntity : IComponentData
    {
        public Entity SpawnObject;

        public float3 Position;
        public quaternion Rotate;
        public float Scale;
    }
}
