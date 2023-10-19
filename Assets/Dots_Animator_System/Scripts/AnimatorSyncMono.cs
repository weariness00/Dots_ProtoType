using Unity.Collections;
using Unity.Entities;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public class AnimatorSyncMono : MonoBehaviour
    {
        public Animator animator;
    }
    
    public class AnimatorSyncBaker : Baker<AnimatorSyncMono>
    {
        public override void Bake(AnimatorSyncMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
        }

        void SetAnimatorFrame(AnimatorSyncMono authoring)
        {
            AnimatorController controller = authoring.animator.runtimeAnimatorController as AnimatorController;

            if (controller == null)
            {
                Debug.LogError("AnimatorController is not assigned.");
                return;
            }

            int layerCount = authoring.animator.layerCount;
            NativeArray<AnimatorSyncLayerAuthoring> animatorSyncLayerAuthorings = new NativeArray<AnimatorSyncLayerAuthoring>(layerCount, Allocator.TempJob);
            for (int i = 0; i < layerCount; i++)
            {
                AnimatorControllerLayer layer = controller.layers[i];
                Debug.LogFormat("Layer {0}: {1}", i, layer.name);
                var animatorSyncLayerAuthoring = animatorSyncLayerAuthorings[i];
                animatorSyncLayerAuthoring.Name = layer.name;
                
                foreach (ChildAnimatorState childState in layer.stateMachine.states)
                {
                    AnimationClip clip = childState.state.motion as AnimationClip;
                    if (clip != null)
                    {
                        // string log = $"Clip Name : {clip.name}\n" +
                        //              $"Clip Length : {clip.length}\n" +
                        //              $"Clip FrameRate : {clip.frameRate}\n" +
                        //              $"Wrap Mode: {clip.wrapMode}\n" +
                        //              $"Legacy: {clip.legacy}\n" +
                        //              $"Events Length: {clip.events.Length}\n" +
                        //              $"Bounds: {clip.localBounds}\n";
                        foreach (var binding in AnimationUtility.GetCurveBindings(clip))
                        {
                            AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                            //
                            // log += $"Property: {binding.propertyName} - Curve Keys: {curve.keys.Length}\n";
                            //
                            // foreach (var keyframe in curve.keys)
                            // {
                            //     log += $"\nkeyFrame Time : {keyframe.time}\n";
                            //     log += $"keyFrame Value : {keyframe.value}\n";
                            // }
                            
                        }
                    }
                }
            }

            void SetAnimatorLayer()
            {
                
            }
        }
    }
}
