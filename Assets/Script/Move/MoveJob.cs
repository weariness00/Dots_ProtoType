using Unity.Entities;
using Unity.Mathematics;

namespace Script.Move
{
    public partial struct MoveJob : IJobEntity
    {
        public float DeltaTime;
        
        private void Execute(MoveAspect moveAspect, [ChunkIndexInQuery]int sortKey)
        {
            float time = moveAspect.MoveAuthoring.ValueRO.Time + DeltaTime;
            moveAspect.MoveAuthoring.ValueRW.Time = time;
            float3 position = new float3(math.sin(time) * 10, 0,math.cos(time) * 10);
            moveAspect.LocalTransform.ValueRW.Position = position;
        }
    }
}
