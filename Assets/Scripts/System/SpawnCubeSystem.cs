using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using static Unity.Entities.SystemAPI.ManagedAPI;

namespace Game
{
    [BurstCompile]
    public partial struct SpawnCubeSystem : ISystem
    {
        float deltaTime;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CubeSpawnerTag>();
            
            // Unity 프로젝트 실행을 했을때 DOTS System이 만들어 준다.
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            deltaTime = 0.0f;
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            var CubeSpawnerAuthoring = SystemAPI.GetComponentRW<CubeSpawnerAuthoring>(SystemAPI.GetSingletonEntity<CubeSpawnerTag>());
            
            if(fps > 60.0f)
                for (int i = 0; i < CubeSpawnerAuthoring.ValueRW.SpawnCubeCount; ++i)
                {
                    ecb.Instantiate(CubeSpawnerAuthoring.ValueRW.CubeEntity);
                    CubeSpawnerAuthoring.ValueRW.SpawnedCubeCount++;
                }
            // Debug.Log(CubeSpawnerAuthoring.ValueRW.SpawnedCubeCount);
            // state.Enabled = false;
        }
    }
}