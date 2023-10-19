using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public struct AnimatorSyncLayerAuthoring : IComponentData
    {
        public FixedString64Bytes Name;
        
        public NativeArray<AnimationClipAuthoring> AnimationClipAuthorings;
    }
}
