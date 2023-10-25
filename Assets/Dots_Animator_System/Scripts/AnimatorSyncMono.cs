using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public class AnimatorSyncMono : MonoBehaviour
    {
        public AnimatorSyncAuthoring GetAnimatorInfo()
        {
            AnimatorSyncAuthoring animatorSync = new AnimatorSyncAuthoring();
            Animator animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError($"Not Have Animator : {gameObject.name}");
                animatorSync.LayerAuthorings = new UnsafeList<AnimatorSyncLayerAuthoring>(0, Allocator.Temp);
                return animatorSync;
            }
        
            AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
            
            UnsafeList<AnimatorSyncLayerAuthoring> animatorSyncLayerAuthorings = new UnsafeList<AnimatorSyncLayerAuthoring>(controller.layers.Length, Allocator.Temp);
            foreach (var layer in controller.layers)
            {
                AnimatorSyncLayerAuthoring animatorSyncLayerAuthoring = new AnimatorSyncLayerAuthoring()
                {
                    Name = layer.name,
                };
        
                UnsafeList<AnimationClipAuthoring> AnimationClipAuthorings = new UnsafeList<AnimationClipAuthoring>(layer.stateMachine.states.Length, Allocator.Temp);
        
                foreach (ChildAnimatorState childState in layer.stateMachine.states)
                {
                    AnimationClip clip = childState.state.motion as AnimationClip;
                    if (clip == null) continue;
        
                    AnimationClipAuthoring clipAuthoring = new AnimationClipAuthoring()
                    {
                        Name = clip.name,
                        Length = clip.length,
                        FrameRate = clip.frameRate,
                        WrapMode = clip.wrapMode,
                        Legacy = clip.legacy,
                        Bounds = clip.localBounds,
                    };
        
                    var binds = AnimationUtility.GetCurveBindings(clip);
                    UnsafeList<AnimationCurveAuthoring> curveAuthorings = new UnsafeList<AnimationCurveAuthoring>(binds.Length, Allocator.Temp);
        
                    foreach (var bind in binds)
                    {
                        AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, bind);
                        
                        AnimationCurveAuthoring animationCurveAuthoring = new AnimationCurveAuthoring()
                        {
                            PropertyName = bind.propertyName,
                            KeyFrames = new UnsafeList<Keyframe>(curve.keys.Length, Allocator.Temp)
                        };

                        foreach (var key in curve.keys) animationCurveAuthoring.KeyFrames.Add(key);
        
                        curveAuthorings.Add(animationCurveAuthoring);
                    }
        
                    clipAuthoring.CurveAuthorings = curveAuthorings;
                    AnimationClipAuthorings.Add(clipAuthoring);
                }
        
                animatorSyncLayerAuthoring.AnimationClipAuthorings = AnimationClipAuthorings;
                animatorSyncLayerAuthorings.Add(animatorSyncLayerAuthoring);
            }

            animatorSync.LayerAuthorings = animatorSyncLayerAuthorings;
            return animatorSync;
        }
    }

    public class AnimatorSyncBaker : Baker<AnimatorSyncMono>
    {
        public override void Bake(AnimatorSyncMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(entity, new AnimatorSync(){ AnimatorSyncAuthoring = authoring.GetAnimatorInfo()});

            // AddSharedComponent(entity,new AnimatorSyncAuthoring()
            // {
            //     LayerAuthorings = authoring.GetAnimatorInfo(),   
            // });
        }
    }
}