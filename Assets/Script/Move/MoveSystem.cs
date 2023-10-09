using Unity.Burst;
using Unity.Entities;

namespace Script.Move
{
    public partial struct MoveSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var moveJob = new MoveJob()
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
            };
            
            state.Dependency = moveJob.ScheduleParallel(state.Dependency);
        }
    }
}
