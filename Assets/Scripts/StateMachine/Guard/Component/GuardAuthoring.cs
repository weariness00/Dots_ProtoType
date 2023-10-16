using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace StateMachine
{
    public struct GuardTag : IComponentData{}
    
    public struct GuardAuthoring : IComponentData
    {
        public float3[] WayPoints;
    }
}