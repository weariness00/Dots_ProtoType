using System;
using Unity.Collections;
using Unity.Entities;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public class AnimatorSyncMono : MonoBehaviour
    {
        public NativeArray<AnimatorSyncLayerAuthoring> GetAnimatorInfo()
        {
            Animator animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError($"Not Have Animator : {gameObject.name}");
                return new NativeArray<AnimatorSyncLayerAuthoring>(0, Allocator.None);
            }
        
            AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
            
            int layerCount = animator.layerCount;
            NativeArray<AnimatorSyncLayerAuthoring> animatorSyncLayerAuthorings = new NativeArray<AnimatorSyncLayerAuthoring>(layerCount, Allocator.Temp);
            for (int i = 0; i < layerCount; i++)
            {
                AnimatorControllerLayer layer = controller.layers[i];
                AnimatorSyncLayerAuthoring animatorSyncLayerAuthoring = new AnimatorSyncLayerAuthoring()
                {
                    Name = layer.name,
                };
        
                NativeArray<AnimationClipAuthoring> AnimationClipAuthorings = new NativeArray<AnimationClipAuthoring>(layer.stateMachine.states.Length, Allocator.Temp);
        
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
                    NativeArray<AnimationCurveAuthoring> curveAuthorings = new NativeArray<AnimationCurveAuthoring>(binds.Length, Allocator.Temp);
        
                    for (int j = 0; j < binds.Length; j++)
                    {
                        AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binds[j]);
                        
                        AnimationCurveAuthoring animationCurveAuthoring = new AnimationCurveAuthoring()
                        {
                            PropertyName = binds[j].propertyName,
                            KeyFrames = new NativeArray<Keyframe>(curve.keys, Allocator.Temp)
                        };
        
                        curveAuthorings[j] = animationCurveAuthoring;
                    }
        
                    clipAuthoring.CurveAuthorings = curveAuthorings;
                    AnimationClipAuthorings[i] = clipAuthoring;
                }
        
                animatorSyncLayerAuthoring.AnimationClipAuthorings = AnimationClipAuthorings;
                animatorSyncLayerAuthorings[i] = animatorSyncLayerAuthoring;
            }
            return animatorSyncLayerAuthorings;
        }
    }

    public class AnimatorSyncBaker : Baker<AnimatorSyncMono>
    {
        public override void Bake(AnimatorSyncMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddSharedComponent(entity,new AnimatorSyncAuthoring()
            {
                LayerAuthorings = authoring.GetAnimatorInfo(),
            });
        }
    }
}