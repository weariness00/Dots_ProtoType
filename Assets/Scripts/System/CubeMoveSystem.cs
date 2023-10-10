using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    public partial struct CubeMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CubeTag>();
        }

        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltatime = SystemAPI.Time.DeltaTime;
            foreach (var (tag, transform,entity) in SystemAPI.Query<CubeTag, RefRW<LocalTransform>>().WithEntityAccess())
            {
                transform.ValueRW = transform.ValueRW.RotateY(6);
                transform.ValueRW = transform.ValueRW.Translate(transform.ValueRW.Forward());
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}