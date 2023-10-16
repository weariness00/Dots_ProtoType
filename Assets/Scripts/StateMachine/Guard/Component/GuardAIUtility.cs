using Unity.Entities;

namespace StateMachine
{
    public struct GuardAIUtility : IComponentData
    {
        public const float StopDistanceSq = 0.4f;
    }
}