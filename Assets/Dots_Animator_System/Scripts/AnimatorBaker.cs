using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace Dots_Animator_System.Scripts
{
    public class AnimatorBaker : MonoBehaviour
    {
        public Animator animator;
        public AnimationClip animationClip;

        private void Start()
        {
            TestAnimator();
        }

        void ClipLog(AnimationClip clip = null)
        {
            if (clip == null) clip = animationClip;
            string log = $"Clip Name : {clip.name}\n" +
                         $"Clip Length : {clip.length}\n" +
                         $"Clip FrameRate : {clip.frameRate}\n" +
                         $"Wrap Mode: {clip.wrapMode}\n" +
                         $"Legacy: {clip.legacy}\n" +
                         $"Events Length: {clip.events.Length}\n" +
                         $"Bounds: {clip.localBounds}\n";
            foreach (var binding in AnimationUtility.GetCurveBindings(clip))
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                
                log += $"Property: {binding.propertyName} - Curve Keys: {curve.keys.Length}\n";

                foreach (var keyframe in curve.keys)
                {
                    log += $"\nkeyFrame Time : {keyframe.time}\n";
                    log += $"keyFrame Value : {keyframe.value}\n";
                }
            }
            Debug.Log(log);
        }

        void TestAnimator()
        {
            AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;

            if (controller == null)
            {
                Debug.LogError("AnimatorController is not assigned.");
                return;
            }

            int layerCount = animator.layerCount;
            for (int i = 0; i < layerCount; i++)
            {
                AnimatorControllerLayer layer = controller.layers[i];
                Debug.LogFormat("Layer {0}: {1}", i, layer.name);

                foreach (ChildAnimatorState childState in layer.stateMachine.states)
                {
                    AnimationClip clip = childState.state.motion as AnimationClip;
                    if (clip != null)
                    {
                        ClipLog(clip);
                    }
                }
            }
        }
    }
}