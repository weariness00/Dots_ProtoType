using Unity.Entities;

namespace Game
{
    public struct RotationComponent : IComponentData
    {
        public float x;
        public float y;
        public float z;
    }
}