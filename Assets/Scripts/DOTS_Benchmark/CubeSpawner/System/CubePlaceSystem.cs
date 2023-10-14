using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DOTS_Benchmark
{
    public partial struct CubePlaceSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<CubeSpawnerTag>();
            state.RequireForUpdate<SpawnCubeTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            
            var cubeSpawnerAuthoring = SystemAPI.GetComponent<CubeSpawnerAuthoring>(SystemAPI.GetSingletonEntity<CubeSpawnerTag>());
            
            foreach (var(tag, transform, cubeAuthoring, entity) in SystemAPI.Query<SpawnCubeTag,RefRW<LocalTransform>, RefRW<CubeAuthoring>>().WithEntityAccess())
            {
                var random = Random.Range(-360, 360);
                cubeAuthoring.ValueRW.Time = random;
                // cubeAuthoring.ValueRW.Radian = math.PI * 2
                
                transform.ValueRW.Position.x = math.cos(cubeAuthoring.ValueRW.Time) * cubeSpawnerAuthoring.Radian;
                transform.ValueRW.Position.z = math.sin(cubeAuthoring.ValueRW.Time) * cubeSpawnerAuthoring.Radian;

                cubeAuthoring.ValueRW.Vector.X = transform.ValueRW.Position.x / cubeSpawnerAuthoring.Radian;
                cubeAuthoring.ValueRW.Vector.Z = transform.ValueRW.Position.z / cubeSpawnerAuthoring.Radian;
                
                ecb.RemoveComponent<SpawnCubeTag>(entity);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}