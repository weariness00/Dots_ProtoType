using Unity.Entities;
using Unity.Transforms;

namespace Script.Spawner
{
    public partial struct SpawnerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public float DeltaTime;
        
        private void Execute(SpawnerAspect aspect, [ChunkIndexInQuery]int sortKey)
        {
            if (aspect.IsSpawn(DeltaTime))
            {
                var newEntity = ECB.Instantiate(sortKey, aspect.SpawnEntity);
                LocalTransform newTransform = new LocalTransform()
                {
                    Position = aspect.LocalTransform.ValueRO.Position,
                    Rotation = aspect.SpawnerEntity.ValueRO.Rotate,
                    Scale = aspect.SpawnerEntity.ValueRO.Scale
                };
                
                ECB.SetComponent(sortKey, newEntity, newTransform);
            }
        }
    }
}
