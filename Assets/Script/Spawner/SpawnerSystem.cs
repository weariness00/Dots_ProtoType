using Unity.Burst;
using Unity.Entities;

namespace Script.Spawner
{
    public partial struct SpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            var spawnerJob = new SpawnerJob()
            {
                ECB = ecb,
                DeltaTime = SystemAPI.Time.DeltaTime,
            };

            state.Dependency = spawnerJob.ScheduleParallel(state.Dependency);
        }
    }
}
