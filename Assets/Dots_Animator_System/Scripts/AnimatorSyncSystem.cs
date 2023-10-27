using Dots_Animator_System.Scripts;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using AnimatorState = Dots_Animator_System.Scripts.AnimatorState;
using AnimatorTransition = Dots_Animator_System.Scripts.AnimatorTransition;

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
        foreach (var (animatorManaged, entity) in SystemAPI.Query<AnimatorManaged>().WithNone<AnimatorSync>().WithEntityAccess())
        {
            var animatorSync = new AnimatorSync()
            {
                Name = animatorManaged.Animator.name,
                AnimatorSyncAuthoring = GetAnimatorSyncAuthoring(animatorManaged.Animator.runtimeAnimatorController as AnimatorController)
            };
            ecb.AddComponent(entity, animatorSync);
        }
        
    }

    AnimatorSyncAuthoring GetAnimatorSyncAuthoring(AnimatorController controller)
    {
        AnimatorSyncAuthoring animatorSyncAuthoring = new AnimatorSyncAuthoring()
        {
            LayerAuthorings = new UnsafeList<AnimatorLayer>(controller.layers.Length, Allocator.Persistent),
        };

        foreach (var layer in controller.layers)
        {
            animatorSyncAuthoring.LayerAuthorings.Add(GetAnimatorLayer(layer));
        }

        return animatorSyncAuthoring;
    }

    AnimatorLayer GetAnimatorLayer(AnimatorControllerLayer layer)
    {
        AnimatorLayer animatorSyncLayerAuthoring = new AnimatorLayer()
        {
            Name = layer.name,
            AnimationClipAuthorings = new UnsafeList<AnimationClipAuthoring>(layer.stateMachine.states.Length, Allocator.Persistent),
            AnimatorStates = new NativeArray<AnimatorState>(layer.stateMachine.states.Length, Allocator.Persistent),
        };

        int i = 0;
        foreach (ChildAnimatorState childState in layer.stateMachine.states)
        {
            animatorSyncLayerAuthoring.AnimationClipAuthorings.Add(GetAnimationClip(childState));
            animatorSyncLayerAuthoring.AnimatorStates[i] = GetAnimatorState(childState.state);
            i++;
        }

        return animatorSyncLayerAuthoring;
    }

    void GetAnimatorTransition(AnimatorControllerLayer layer)
    {
        foreach (var childAnimatorStateMachine in layer.stateMachine.stateMachines)
        {
            var transitions = layer.stateMachine.GetStateMachineTransitions(childAnimatorStateMachine.stateMachine);
            foreach (var transition in transitions)
            {
                var animatorTransition = new AnimatorTransition()
                {
                    Name = transition.name,
                };
            }
        }
    }
    
    AnimatorState GetAnimatorState(UnityEditor.Animations.AnimatorState state)
    {
        AnimatorState animatorState = new AnimatorState()
        {
            Name = state.name,
            Transitions = GetAnimatorStateTransition(state),
            Speed = state.speed,
            SpeedMultiplierParameter = state.speedParameter,
            TimeParameter = state.timeParameter,
            CycleOffset = state.cycleOffset,
            CycleOffsetParameter = state.cycleOffsetParameter
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