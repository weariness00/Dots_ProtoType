using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace StateMachine
{
    public partial struct Guard_MoveToTargetSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = Time.deltaTime;

            foreach (var(guardTag, localTransform, guardAuthoring, movementSpeed) in SystemAPI.Query<GuardTag, RefRW<LocalTransform>, RefRW<GuardAuthoring>, RefRW<MovementSpeed>>().WithNone<IdleTimer>())
            {
                float3 VectorToTarget = new float3(0, 0, 0);
                //남은 거리
                if (guardAuthoring.ValueRW.direction == 0)
                {
                    VectorToTarget = guardAuthoring.ValueRW.WayPoints1 - localTransform.ValueRW.Position;
                }
                else
                {
                    VectorToTarget = guardAuthoring.ValueRW.WayPoints2 - localTransform.ValueRW.Position;
                }
                
                // 도착여부 확인
                if (math.lengthsq(VectorToTarget) > GuardAIUtility.StopDistanceSq)
                {
                    var moveDirection = math.normalize(VectorToTarget);
                    
                    //위치, 방향 변경
                    localTransform.ValueRW.Rotation =
                        quaternion.LookRotation(new float3(moveDirection.x, 0.0f, moveDirection.y), math.up());
                    localTransform.ValueRW.Position += moveDirection * movementSpeed.ValueRW.MeterPerSecond * deltaTime;
                }
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}