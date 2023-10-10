using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    [BurstCompile]
    public partial struct SpawnNoteSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CubeSpawnerTag>();
            // Unity 프로젝트 실행을 했을때 DOTS System이 만들어 준다.
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            var noteSpawnerAuthoring = SystemAPI.GetComponent<CubeSpawnerAuthoring>(SystemAPI.GetSingletonEntity<CubeSpawnerTag>());
            for (int i = 0; i < noteSpawnerAuthoring.CubeCount; ++i)
            {
                ecb.Instantiate(noteSpawnerAuthoring.CubeEntity);
            }

            state.Enabled = false;
        }
    }
}