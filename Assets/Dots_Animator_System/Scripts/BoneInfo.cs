using Unity.Collections;
using Unity.Entities;

namespace Dots_Animator_System.Scripts
{
    public struct BoneInfo : IBufferElementData
    {
        public FixedString128Bytes Name;
        public Entity Entity;
    }
}
