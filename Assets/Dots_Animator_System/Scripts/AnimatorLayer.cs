
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Dots_Animator_System.Scripts
{
    public struct AnimatorLayer : IDisposable
    {
        public FixedString128Bytes Name;
        
        public UnsafeList<AnimationClipAuthoring> AnimationClipAuthorings;
        public NativeArray<AnimatorState> AnimatorStates;

        public void Dispose()
        {
            foreach (var clip in AnimationClipAuthorings) clip.Dispose();
            foreach (var s in AnimatorStates) s.Dispose();
            
            AnimationClipAuthorings.Dispose();
            AnimatorStates.Dispose();
        }
    }
}