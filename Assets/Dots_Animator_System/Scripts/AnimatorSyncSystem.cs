using Dots_Animator_System.Scripts;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public partial struct AnimatorSyncSystem : ISystem
{
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
        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (animatorManaged, entity) in SystemAPI.Query<AnimatorManaged>().WithEntityAccess())
        {
            ecb.AddComponent(entity, new AnimatorSync()
            {
                Name = animatorManaged.Animator.name,
                AnimatorSyncAuthoring = GetAnimatorSyncAuthoring(animatorManaged.Animator.runtimeAnimatorController as AnimatorController)
            });
            
            ecb.RemoveComponent<AnimatorManaged>(entity);
        }
    }

    AnimatorSyncAuthoring GetAnimatorSyncAuthoring(AnimatorController controller)
    {
        AnimatorSyncAuthoring animatorSyncAuthoring = new AnimatorSyncAuthoring()
        {
            LayerAuthorings = new UnsafeList<AnimatorSyncLayerAuthoring>(controller.layers.Length, Allocator.Persistent),
        };

        foreach (var layer in controller.layers)
        {
            animatorSyncAuthoring.LayerAuthorings.Add(GetAnimatorLayer(layer));
        }

        return animatorSyncAuthoring;
    }

    AnimatorSyncLayerAuthoring GetAnimatorLayer(AnimatorControllerLayer layer)
    {
        AnimatorSyncLayerAuthoring animatorSyncLayerAuthoring = new AnimatorSyncLayerAuthoring()
        {
            Name = layer.name,
            AnimationClipAuthorings = new UnsafeList<AnimationClipAuthoring>(layer.stateMachine.states.Length, Allocator.Persistent)
        };

        foreach (ChildAnimatorState childState in layer.stateMachine.states)
        {
            animatorSyncLayerAuthoring.AnimationClipAuthorings.Add(GetAnimationClip(childState));
        }

        return animatorSyncLayerAuthoring;
    }

    AnimationClipAuthoring GetAnimationClip(ChildAnimatorState childState)
    {
        AnimationClip clip = childState.state.motion as AnimationClip;
        if (clip == null) return new AnimationClipAuthoring();

        var binds = AnimationUtility.GetCurveBindings(clip);
        AnimationClipAuthoring clipAuthoring = new AnimationClipAuthoring()
        {
            Name = clip.name,
            Length = clip.length,
            FrameRate = clip.frameRate,
            WrapMode = clip.wrapMode,
            Legacy = clip.legacy,
            Bounds = clip.localBounds,
            CurveAuthorings = new UnsafeList<AnimationCurveAuthoring>(binds.Length, Allocator.Persistent)
        };

        foreach (var bind in binds) clipAuthoring.CurveAuthorings.Add(GetAnimationCurve(bind, clip));

        return clipAuthoring;
    }

    AnimationCurveAuthoring GetAnimationCurve(EditorCurveBinding bind, AnimationClip clip)
    {
        AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, bind);

        AnimationCurveAuthoring animationCurveAuthoring = new AnimationCurveAuthoring()
        {
            PropertyName = bind.propertyName,
            KeyFrames = new UnsafeList<Keyframe>(curve.keys.Length, Allocator.Persistent)
        };

        foreach (var key in curve.keys) animationCurveAuthoring.KeyFrames.Add(key);

        return animationCurveAuthoring;
    }
}