using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace StateMachine
{
    public struct GuardTag : IComponentData{}
    
    public struct GuardAuthoring : IComponentData
    {
        public int direction;
        
        public float3 WayPoints1;
        public float3 WayPoints2;
    }
}