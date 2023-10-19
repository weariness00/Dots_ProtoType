using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public struct AnimationClipBuffer : IBufferElementData
    {
        public AnimationClipAuthoring Clip;
    }
    
    public struct AnimationClipAuthoring : ISharedComponentData
    {
        public FixedString64Bytes Name;
        public float Length;
        public float FrameRate;
        public WrapMode WrapMode;
        public bool Legacy;
        public Bounds Bounds;
    }

    // public struct AnimationEvent : IBufferElementData
    // {
    //     
    // }
}
