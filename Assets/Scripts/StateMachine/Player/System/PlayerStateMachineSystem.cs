using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace StateMachine
{
    public partial struct PlayerStateMachineSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            
            // 쿼리에 playerTag와 같은 쓸데없는 놈들 넣지마
            foreach (var(playerTag, userInputData, userEntity) in SystemAPI.Query<PlayerTag, RefRW<UserInputData>>().WithNone<PlayerRunStateTag>().WithEntityAccess())
            {
                if (userInputData.ValueRW.Move.x != 0 || userInputData.ValueRW.Move.y != 0)
                {
                    ecb.RemoveComponent<PlayerIdleStateTag>(userEntity);
                    ecb.AddComponent<PlayerRunStateTag>(userEntity);
                }
            }
            
            foreach (var(playerTag, userInputData, userEntity) in SystemAPI.Query<PlayerTag, RefRW<UserInputData>>().WithNone<PlayerIdleStateTag>().WithEntityAccess())
            {
                if (userInputData.ValueRW.Move.x == 0 && userInputData.ValueRW.Move.y == 0)
                {
                    ecb.RemoveComponent<PlayerRunStateTag>(userEntity);
                    ecb.AddComponent<PlayerIdleStateTag>(userEntity);
                }
            }
            
            // var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            // var playerInputData = SystemAPI.GetComponent<UserInputData>(playerEntity);
            //
            // if (playerInputData.Move.x != 0 && playerInputData.Move.y != 0)
            // {
            //     ecb.AddComponent<PlayerRunStateTag>(playerEntity);
            // }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}