using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace StateMachine
{
    public class GuardMono : MonoBehaviour
    {
        public float MovementSpeedMetersPerSecond = 3.0f;
        
        public float3 WayPoints1;
        public float3 WayPoints2;
    }
    public class GuardBaker : Baker<GuardMono>
    {
        public override void Bake(GuardMono authoring)
        {
            var guardEntity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);

            AddComponent<GuardTag>(guardEntity);
            
            AddComponent(guardEntity, new GuardAuthoring()
            {
                direction = 0,
                WayPoints1 = authoring.WayPoints1,
                WayPoints2 = authoring.WayPoints2,
            });
            AddComponent(guardEntity, new GuardAIUtility());
            
            // var EM = World.DefaultGameObjectInjectionWorld.EntityManager;
            // var Buffer = EM.GetBuffer<WaypointPosition>(guardEntity);
            // AddComponent(guardEntity, new TargetPosition()
            // {
            //     Value = Buffer[0].Value,
            // });

            AddComponent(guardEntity, new MovementSpeed()
            {
                MeterPerSecond = authoring.MovementSpeedMetersPerSecond,
            });
            
            // AddComponent(guardEntity, new WaypointPosition()
            // {
            //     Value = Buffer[0].Value,
            // });
        }
    }
}