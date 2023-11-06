using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public struct AnimatorStateJob : IJobParallelForBatch
    {
        public BufferLookup<AnimatorControllerBlobAssetReference> ControllerBufferLookup;
        public NativeArray<Entity> Entities;
        
        public void Execute(int startIndex, int count)
        {
            for (int i = startIndex; i < startIndex + count; i++)
            {
                PlayAnimation(i);
            }
        }

        void PlayAnimation(int index)
        {
            var controllerBuffer = ControllerBufferLookup[Entities[index]];
            foreach (var controller in controllerBuffer)
            {
                if(controller.LayerWeight == 0) continue;

                ref AnimationClipBlob clip = ref controller.AnimatorReference.Value.AnimationClipArray[0];
                
                for (int i = 0; i < controller.AnimatorReference.Value.AnimationClipArray.Length; i++)
                {
                    if (controller.AnimatorReference.Value.AnimationClipArray[i].Equals(controller.CurrenStateBlob.AnimationClipHashCode))
                    {
                        clip = controller.AnimatorReference.Value.AnimationClipArray[i];
                        break;
                    }
                }

                for (int i = 0; i < clip.Curves.Length; i++)
                {
                    
                }
            }
        }
    }
}
