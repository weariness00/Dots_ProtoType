using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace StateMachine
{
    public partial struct PlayerMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<PlayerRunStateTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = Time.deltaTime;

            foreach (var(playerTag, playerRunStateTag, localTransform, inputData, movementSpeed) in SystemAPI.Query<PlayerTag, PlayerRunStateTag, RefRW<LocalTransform>, RefRW<UserInputData>, RefRW<MovementSpeed>>())
            {
                localTransform.ValueRW.Position += new float3(inputData.ValueRW.Move.x, 0.0f, inputData.ValueRW.Move.y) * movementSpeed.ValueRW.MeterPerSecond * deltaTime;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}