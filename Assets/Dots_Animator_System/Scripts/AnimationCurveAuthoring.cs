using Unity.Collections;
using Unity.Entities;

namespace Dots_Animator_System.Scripts
{
    public struct AnimationCurveAuthoring : IComponentData
    {
        public FixedString64Bytes PropertyName;

        public NativeArray<KeyFrameAuthoring> KeyFrames;
    }
}
