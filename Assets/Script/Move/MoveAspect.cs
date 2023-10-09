using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Transforms;

namespace Script.Move
{
    public readonly partial struct MoveAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<MoveTag> _moveTag;

        public readonly RefRW<LocalTransform> LocalTransform;
        public readonly RefRW<MoveAuthoring> MoveAuthoring;
    }
}
