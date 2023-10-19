using Unity.Entities;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public struct KeyFrameAuthoring : IComponentData
    {
        public Keyframe Value;
    }
}
