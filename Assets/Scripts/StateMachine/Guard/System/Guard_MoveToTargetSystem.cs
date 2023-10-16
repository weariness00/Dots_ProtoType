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

            foreach (var(guardTag, localTransform, targetPosition, movementSpeed) in SystemAPI.Query<GuardTag, RefRW<LocalTransform>, RefRW<TargetPosition>, RefRW<MovementSpeed>>())
            {
                //남은 거리
                var VectorToTarget = targetPosition.ValueRW.Value - localTransform.ValueRW.Position;

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