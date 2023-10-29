using System;
using Unity.Collections;
using Unity.Entities;

namespace Dots_Animator_System.Scripts
{
    // AnimatorStateTransition을 이용해야됨
    // Transition의 전환 조건이 되는 AnimatorCondition 사용 

    public struct AnimatorTransitionBlob
    {
        public FixedString512Bytes Name;
        public float Duration;
        public float ExitTime;
        public float Offset;
        public bool HasExitTime;
        public bool HasFixedDuration;
        public bool SoloFlag;
        public bool MuteFlag;
        public bool CanTransitionToSelf;
        public int TargetStateHash;

        public BlobArray<AnimatorCondition> Conditions;

        public AnimatorTransitionBlob(AnimatorTransition transition, BlobBuilder blobBuilder)
        {
            this = blobBuilder.ConstructRoot<AnimatorTransitionBlob>();

            Name = transition.Name;
            Duration = transition.Duration;
            ExitTime = transition.ExitTime;
            Offset = transition.Offset;
            HasExitTime = transition.HasExitTime;
            HasFixedDuration = transition.HasFixedDuration;
            SoloFlag = transition.SoloFlag;
            MuteFlag = transition.MuteFlag;
            CanTransitionToSelf = transition.CanTransitionToSelf;
            TargetStateHash = transition.TargetStateHash;

            var conditionLength = transition.Conditions.Length;
            if (conditionLength > 0)
            {
                var blobConditionArray = blobBuilder.Allocate(ref this.Conditions, conditionLength);
                for (int i = 0; i < conditionLength; i++) blobConditionArray[i] = transition.Conditions[i];
            }
        }
    }

    public struct AnimatorTransition : IDisposable
    {
        public FixedString512Bytes Name;
        public float Duration;
        public float ExitTime;
        public float Offset;
        public bool HasExitTime;
        public bool HasFixedDuration;
        public bool SoloFlag;
        public bool MuteFlag;
        public bool CanTransitionToSelf;
        public int TargetStateHash;
        public NativeArray<AnimatorCondition> Conditions;

        public void Dispose()
        {
            Conditions.Dispose();
        }

        public AnimatorTransition(UnityEditor.Animations.AnimatorStateTransition transition)
        {
            Name = transition.name;
            Duration = transition.duration;
            ExitTime = transition.exitTime;
            Offset = transition.offset;
            HasExitTime = transition.hasExitTime;
            HasFixedDuration = transition.hasFixedDuration;
            SoloFlag = transition.solo;
            MuteFlag = transition.mute;
            CanTransitionToSelf = transition.canTransitionToSelf;

            TargetStateHash = transition.destinationState.GetHashCode();

            Conditions = new NativeArray<AnimatorCondition>(transition.conditions.Length, Allocator.Persistent);
            for (int i = 0; i < transition.conditions.Length; i++)
            {
                Conditions[i] = new AnimatorCondition()
                {
                    ConditionMode = transition.conditions[i].mode,
                    ConditionEvent = transition.conditions[i].parameter,
                    EventTreshold = transition.conditions[i].threshold,
                };
            }
        }
    }
}