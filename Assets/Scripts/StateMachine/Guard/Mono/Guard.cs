﻿using Unity.Entities;
using UnityEngine;

namespace StateMachine
{
    public class GuardMono : MonoBehaviour
    {
        public float MovementSpeedMetersPerSecond = 3.0f;
    }
    public class GuardBaker : Baker<GuardMono>
    {
        public override void Bake(GuardMono authoring)
        {
            var guardEntity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);

            AddComponent<GuardTag>(guardEntity);

            EntityManager EM;
            var Buffer = EM.GetBuffer<WaypointPosition>(guardEntity);
            
            AddComponent(guardEntity, new GuardAuthoring()
            {
                    
            });
            
            AddComponent(guardEntity, new GuardAIUtility());
            AddComponent(guardEntity, new TargetPosition());
            AddComponent(guardEntity, new WaypointPosition()
            {

            });
            AddComponent(guardEntity, new MovementSpeed()
            {
                MeterPerSecond = authoring.MovementSpeedMetersPerSecond,
            });
        }
    }
}