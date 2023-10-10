using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public partial struct NotePlaceSystem : ISystem
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

            var noteSpawnerAuthoring = SystemAPI.GetComponent<CubeSpawnerAuthoring>(SystemAPI.GetSingletonEntity<CubeSpawnerTag>());
            
            foreach (var(tag, transform, entity) in SystemAPI.Query<SpawnCubeTag,RefRW<LocalTransform>>().WithEntityAccess())
            {
                var xrandom = Random.Range(-10, 10);
                var zrandom = Random.Range(-10, 10);
                transform.ValueRW.Position = new float3(xrandom, 0, zrandom);
                
                // Debug.Log(noteSpawnerAuthoring.Radian);
                
                ecb.RemoveComponent<SpawnCubeTag>(entity);
            }

            state.Enabled = false;
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}