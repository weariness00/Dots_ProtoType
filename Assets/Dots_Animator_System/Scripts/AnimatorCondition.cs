
using Unity.Collections;
using UnityEditor.Animations;

namespace Dots_Animator_System.Scripts
{
    public struct AnimatorCondition
    {
        public AnimatorConditionMode ConditionMode;
        public FixedString128Bytes ConditionEvent;
        public float EventTreshold;
    }
}