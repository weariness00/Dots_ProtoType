using System;
using Unity.Collections;
using Unity.Entities;

namespace Dots_Animator_System.Scripts 
{
    public struct AnimatorState : IEquatable<int>, IDisposable
    {
        public FixedString128Bytes Name;
        public NativeArray<AnimatorTransition> Transitions;
        public float Speed;
        public FixedString128Bytes SpeedMultiplierParameter;
        public FixedString128Bytes TimeParameter;
        public float CycleOffset;
        public FixedString128Bytes CycleOffsetParameter;
        // public Motion motion;

        public int HashCode => GetHashCode();
        public bool Equals(int hash) => hash == HashCode;
        public void Dispose()
        {
            foreach (var t in Transitions) t.Dispose();
            Transitions.Dispose();
        }
    }
}
