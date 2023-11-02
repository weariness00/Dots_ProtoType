using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Dots_Animator_System.Scripts
{
    [StructLayout(LayoutKind.Auto)]
    public partial struct AnimatorContolSystem : ISystem
    {
        private EntityQuery _entityQuery;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();

            using var entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<AnimatorTag>();
            _entityQuery = state.GetEntityQuery(entityQueryBuilder);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entities = _entityQuery.ToEntityArray(state.WorldUpdateAllocator);
            var controlJob = new AnimatorStateJob()
            {
                ControllerBufferLookup = SystemAPI.GetBufferLookup<AnimatorControllerBlobAssetReference>(),
                Entities = entities,
            };
            
            var jobHandle = controlJob.Schedule(entities.Length, 16); // 64는 배치 크기입니다.
            jobHandle.Complete();
        }
    }
}
    