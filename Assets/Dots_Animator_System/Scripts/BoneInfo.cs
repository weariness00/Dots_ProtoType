using Unity.Collections;
using Unity.Entities;

namespace Dots_Animator_System.Scripts
{
    public struct BoneInfoBlob
    {
        public FixedString128Bytes Name;
        public Entity Entity;

        public void MakeBlob(BoneInfo boneInfo)
        {
            Name = boneInfo.Name;
            Entity = boneInfo.Entity;
        }
    }

    public struct BoneInfo : IBufferElementData
    {
        public FixedString128Bytes Name;
        public Entity Entity;
    }
}
