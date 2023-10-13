using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    [UpdateAfter(typeof(CubePlaceSystem))]
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
            
            var cubeSpawnerEntity = SystemAPI.GetSingletonEntity<CubeSpawnerTag>();
            var cubeSpawnerAuthoring = SystemAPI.GetComponent<CubeSpawnerAuthoring>(cubeSpawnerEntity);
            
            foreach (var (tag, transform, cubeAuthoring, entity) in SystemAPI.Query<CubeTag, RefRW<LocalTransform>, RefRW<CubeAuthoring>>().WithEntityAccess())
            {
                // cubeAuthoring.ValueRW.Time += deltatime * cubeSpawnerAuthoring.Speed;
                //
                // transform.ValueRW.Position.x = math.cos(cubeAuthoring.ValueRW.Time) * cubeSpawnerAuthoring.Radian;
                // transform.ValueRW.Position.z = math.sin(cubeAuthoring.ValueRW.Time) * cubeSpawnerAuthoring.Radian;

                transform.ValueRW.Position.x += cubeAuthoring.ValueRW.Vector.X * deltatime * cubeSpawnerAuthoring.Speed;
                transform.ValueRW.Position.z += cubeAuthoring.ValueRW.Vector.Z * deltatime * cubeSpawnerAuthoring.Speed;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}