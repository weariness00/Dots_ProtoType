using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace StateMachine
{
    public partial struct CehckReachedWaypointSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var(guardTag, localTransform, targetPosition, guardEntity) in SystemAPI.Query<GuardTag, RefRW<LocalTransform>, RefRW<TargetPosition>>().WithNone<IdleTimer>().WithEntityAccess())
            {
                var distanceSq = math.lengthsq(targetPosition.ValueRW.Value - localTransform.ValueRW.Position);

                if (distanceSq < GuardAIUtility.StopDistanceSq)
                {
                    ecb.AddComponent<IdleTimer>(guardEntity);
                }
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}