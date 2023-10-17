using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace Dots_Animator_System.Scripts
{
    public class AnimatorBaker : MonoBehaviour
    {
        public Animator animator;

        private void Start()
        {
            var controller = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;

            controller.layers[0].stateMachine;
            foreach (var layer in controller.layers) 
            {
                // Debug.Log(layer.stateMachine.defaultState.name);    // 일반 이름
                // Debug.Log(layer.stateMachine.defaultState);         // AnimatorState  
                // Debug.Log(layer.stateMachine.defaultState.motion);  // AnimationClip
                //
                // var cliip = layer.stateMachine.defaultState.motion as AnimationClip;
                // Debug.Log(cliip.name + " clip");

                
                
                foreach (var stateMachine in layer.stateMachine.stateMachines)
                {
                    Debug.Log(stateMachine.stateMachine.defaultState.name);
                }
                
                // foreach (var animatorState in layer.stateMachine.name)
                // {
                //     Debug.Log(animatorState);
                // }
            }
            
            // foreach (var clips in animator.runtimeAnimatorController.animationClips)
            // {
            //     Debug.Log(clips.name);
            // }
        }
    }
}