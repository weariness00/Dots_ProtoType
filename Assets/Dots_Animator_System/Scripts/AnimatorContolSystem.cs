using Unity.Burst;
using Unity.Entities;

namespace Dots_Animator_System.Scripts
{
    public partial struct AnimatorContolSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var animatorControllerBlobAssetReferenceBufferLookup = SystemAPI.GetBufferLookup<AnimatorControllerBlobAssetReference>();
            
            // var controlJob = new AnimatorStateJob()
            // {
            //
            // };
            //
            // state.Dependency = controlJob.ScheduleParallel(state.Dependency);
        }
    }
}
    