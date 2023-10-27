using System;
using Unity.Collections;

namespace Dots_Animator_System.Scripts
{
    // AnimatorStateTransition을 이용해야됨
    // Transition의 전환 조건이 되는 AnimatorCondition 사용 
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
        public int TargetStateHash; // its not use
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
            
            // TargetStateHash = transition.;
            TargetStateHash = 0;

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
