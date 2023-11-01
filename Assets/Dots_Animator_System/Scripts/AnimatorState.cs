using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace Dots_Animator_System.Scripts
{
    public struct AnimatorStateBlob : IEquatable<int>
    {
        public FixedString128Bytes Name;
        public float Speed;
        public FixedString128Bytes SpeedMultiplierParameter;
        public FixedString128Bytes TimeParameter;
        public float CycleOffset;
        public FixedString128Bytes CycleOffsetParameter;

        public BlobArray<AnimatorTransitionBlob> Transitions;
        public int NameHashCode;

        public bool Equals(int hash) => hash == NameHashCode;
        
        public void MakeBlob(AnimatorState state, BlobBuilder blobBuilder)
        {
            Name = state.Name;
            Speed = state.Speed;
            SpeedMultiplierParameter = state.SpeedMultiplierParameter;
            TimeParameter = state.TimeParameter;
            CycleOffset = state.CycleOffset;
            CycleOffsetParameter = state.CycleOffsetParameter;
            NameHashCode = state.NameHashCode;

            var transitionLenght = state.Transitions.Length;
            if (transitionLenght > 0)
            {
                var blobTransitionArray = blobBuilder.Allocate(ref this.Transitions, transitionLenght);
                for (int i = 0; i < transitionLenght; i++) blobTransitionArray[i].MakeBlob(state.Transitions[i], blobBuilder);
            }
        }
    }

    public struct AnimatorState : IEquatable<int>, IDisposable
    {
        public FixedString128Bytes Name;
        public int AniamtionClipHashCode;
        public NativeArray<AnimatorTransition> Transitions;
        public float Speed;
        public FixedString128Bytes SpeedMultiplierParameter;
        public FixedString128Bytes TimeParameter;
        public float CycleOffset;

        public FixedString128Bytes CycleOffsetParameter;
        // public Motion motion;
        public int NameHashCode;

        public bool Equals(int hash) => hash == NameHashCode;

        public void Dispose()
        {
            foreach (var t in Transitions) t.Dispose();
            Transitions.Dispose();
        }
    }
}