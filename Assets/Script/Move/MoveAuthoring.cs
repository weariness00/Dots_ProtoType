using Unity.Entities;

namespace Script.Move
{
    public struct MoveTag : IComponentData{}
    
    public struct MoveAuthoring : IComponentData
    {
        public float Speed;
        public float Time;
    }
}
