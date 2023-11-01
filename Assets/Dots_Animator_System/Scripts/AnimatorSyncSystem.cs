using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Transforms;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public partial struct AnimatorSyncSystem : ISystem
    {
        private EntityCommandBuffer ecb;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var (animatorManaged, entity) in SystemAPI.Query<AnimatorManaged>().WithNone<AnimatorSync>().WithEntityAccess())
            {
                var animatorSync = GetAnimatorSync(animatorManaged.Animator.runtimeAnimatorController as AnimatorController);
                ecb.AddComponent(entity, animatorSync);
                GetBoneEntity(entity);
            }
        }

        void GetBoneEntity(Entity entity)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var boneInfoBuffer = ecb.AddBuffer<BoneInfo>(entity);
            boneInfoBuffer.Add(new BoneInfo(){Entity = entity, Name = entityManager.GetName(entity)});
            GetChild(entityManager, entity, ref boneInfoBuffer);
        }

        void GetChild(EntityManager entityManager, Entity entity, ref DynamicBuffer<BoneInfo> boneInfoBuffer)
        {
            if (entityManager.HasBuffer<Child>(entity))
            {
                var entityBuffer = entityManager.GetBuffer<Child>(entity);
                foreach (var child in entityBuffer)
                {
                    boneInfoBuffer.Add(new BoneInfo()
                    {
                        Entity = child.Value,
                        Name = entityManager.GetName(child.Value)
                    });

                    GetChild(entityManager, child.Value, ref boneInfoBuffer);
                }
            }
        }

        AnimatorSync GetAnimatorSync(AnimatorController controller)
        {
            AnimatorSync animatorSync = new AnimatorSync()
            {
                Name =  controller.name,
                LayerAuthorings = new UnsafeList<AnimatorLayer>(controller.layers.Length, Allocator.Persistent),
            };

            foreach (var layer in controller.layers)
            {
                animatorSync.LayerAuthorings.Add(GetAnimatorLayer(layer));
            }

            return animatorSync;
        }

        AnimatorLayer GetAnimatorLayer(AnimatorControllerLayer layer)
        {
            AnimatorLayer animatorSyncLayerAuthoring = new AnimatorLayer()
            {
                Name = layer.name,
                AnimationClip = new UnsafeList<AnimationClip>(layer.stateMachine.states.Length, Allocator.Persistent),
                AnimatorStates = new NativeArray<AnimatorState>(layer.stateMachine.states.Length, Allocator.Persistent),
                CurrentStateHashCode = layer.stateMachine.defaultState.nameHash,
                DefaultStateHashCode = layer.stateMachine.defaultState.nameHash,
            };
        
            int i = 0;
            foreach (ChildAnimatorState childState in layer.stateMachine.states)
            {
                var clip = GetAnimationClip(childState);
                animatorSyncLayerAuthoring.AnimationClip.Add(clip);
                animatorSyncLayerAuthoring.AnimatorStates[i] = GetAnimatorState(childState.state, clip);
                i++;
            }

            return animatorSyncLayerAuthoring;
        }

        AnimatorState GetAnimatorState(UnityEditor.Animations.AnimatorState state, AnimationClip clip)
        {
            AnimatorState animatorState = new AnimatorState()
            {
                Name = state.name,
                AniamtionClipHashCode = clip.GetHashCode(),
                Transitions = GetAnimatorStateTransition(state),
                Speed = state.speed,
                SpeedMultiplierParameter = state.speedParameter,
                TimeParameter = state.timeParameter,
                CycleOffset = state.cycleOffset,
                CycleOffsetParameter = state.cycleOffsetParameter,
                
                NameHashCode = state.nameHash,
            };
            return animatorState;
        }

        NativeArray<AnimatorTransition> GetAnimatorStateTransition(UnityEditor.Animations.AnimatorState state)
        {
            NativeArray<AnimatorTransition> transitions = new NativeArray<AnimatorTransition>(state.transitions.Length, Allocator.Persistent);
        
            for (int i = 0; i < state.transitions.Length; i++)
            {
                transitions[i] = new AnimatorTransition(state.transitions[i]);
            }

            return transitions;
        }

        AnimationClip GetAnimationClip(ChildAnimatorState childState)
        {
            UnityEngine.AnimationClip clip = childState.state.motion as UnityEngine.AnimationClip;
            if (clip == null) return new AnimationClip();

            var binds = AnimationUtility.GetCurveBindings(clip);
            AnimationClip clipAuthoring = new AnimationClip()
            {
                Name = clip.name,
                Length = clip.length,
                FrameRate = clip.frameRate,
                WrapMode = clip.wrapMode,
                Legacy = clip.legacy,
                Bounds = clip.localBounds,
                CurveAuthorings = new UnsafeList<AnimationCurve>(binds.Length, Allocator.Persistent)
            };

            foreach (var bind in binds) clipAuthoring.CurveAuthorings.Add(GetAnimationCurve(bind, clip));

            return clipAuthoring;
        }

        AnimationCurve GetAnimationCurve(EditorCurveBinding bind, UnityEngine.AnimationClip clip)
        {
            UnityEngine.AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, bind);

            AnimationCurve animationCurveAuthoring = new AnimationCurve()
            {
                PropertyName = bind.propertyName,
                KeyFrames = new UnsafeList<Keyframe>(curve.keys.Length, Allocator.Persistent)
            };

            foreach (var key in curve.keys) animationCurveAuthoring.KeyFrames.Add(key);

            return animationCurveAuthoring;
        }
    }
}