using Unity.Entities;
using Unity.Transforms;

namespace StateMachine
{
    public readonly partial struct PlayerAspect : IAspect
    {
        public readonly RefRW<LocalTransform> LocalTransform;
    }
}