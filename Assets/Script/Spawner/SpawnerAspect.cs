using Unity.Entities;
using Unity.Transforms;

namespace Script.Spawner
{
    public readonly partial struct SpawnerAspect : IAspect
    {
        public readonly Entity Entity;

        public readonly RefRW<LocalTransform> LocalTransform;
        
        public readonly RefRW<SpawnerAuthoring> SpawnerAuthoring;
        public readonly RefRO<SpawnerEntity> SpawnerEntity;

        private float CurrentTimer
        {
            get => SpawnerAuthoring.ValueRO.CurrentTimer;
            set => SpawnerAuthoring.ValueRW.CurrentTimer = value;
        }

        public Entity SpawnEntity => SpawnerEntity.ValueRO.SpawnObject;

        public bool IsSpawn(float time)
        {
            CurrentTimer += time;
            if ( SpawnerAuthoring.ValueRO.SpawnIndex > SpawnerAuthoring.ValueRO.SpawnNumber &&
                 CurrentTimer > SpawnerAuthoring.ValueRO.SpawnInterval)
            {
                CurrentTimer = 0;

                SpawnerAuthoring.ValueRW.SpawnNumber += 1;
                return true;
            }
            return false;
        }
    }
}
