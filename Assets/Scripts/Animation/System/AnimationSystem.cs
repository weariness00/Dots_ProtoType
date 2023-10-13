using Game;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Game
{
    [UpdateInGroup(typeof(PresentationSystemGroup), OrderFirst = true)]
    
    public partial struct DotsAnimateSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CubeSpawnerTag>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            var noteSpawnerAuthoring = SystemAPI.GetComponent<CubeSpawnerAuthoring>(SystemAPI.GetSingletonEntity<CubeSpawnerTag>());
            
                foreach (var (playerGameObjectPrefab, entity) in
                         SystemAPI.Query<AnimatorSyncObject>().WithNone<DotsAnimatorAuthoring>().WithEntityAccess())
                {
                    var newCompanionGameObject = Object.Instantiate(playerGameObjectPrefab.gameObject);
                    var newAnimatorReference = new DotsAnimatorAuthoring
                    {
                        ani = newCompanionGameObject.GetComponent<Animator>()
                    };
                    ecb.AddComponent(entity, newAnimatorReference);
                }
                
                foreach (var (transform, animatorReference) in 
                         SystemAPI.Query<LocalTransform, DotsAnimatorAuthoring>())
                {
                    animatorReference.ani.transform.position = transform.Position;
                    animatorReference.ani.transform.rotation = transform.Rotation;
                }
            
            foreach (var (transform, animatorReference) in 
                     SystemAPI.Query<RefRW<LocalTransform>, DotsAnimatorAuthoring>())
            {
                var anitransform = animatorReference.ani.transform;
                transform.ValueRW.Position = anitransform.position;
                transform.ValueRW.Rotation = anitransform.rotation;
            }
            
            foreach (var (animationReference, entity) in
                     SystemAPI.Query<DotsAnimatorAuthoring>().WithNone<AnimatorSyncObject, LocalTransform>()
                         .WithEntityAccess())
            {
                Object.Destroy(animationReference.ani.gameObject);

                ecb.RemoveComponent<DotsAnimatorAuthoring>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}