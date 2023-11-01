using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace Dots_Animator_System.Scripts
{
    public struct AnimatorLayerBlob
    {
        public FixedString128Bytes Name;

        public BlobArray<AnimationClipBlob> AnimationClips;
        public BlobArray<AnimatorStateBlob> AnimatorStates;
        public int CurrentStateHashCode;
        public int DefaultStateHashCode;

        public void MakeBlob(AnimatorLayer layer, BlobBuilder blobBuilder)
        {
            Name = layer.Name;
            CurrentStateHashCode = layer.CurrentStateHashCode;
            DefaultStateHashCode = layer.DefaultStateHashCode;
            
            if (layer.AnimationClip.Length > 0)
            {
                var blobClipArray = blobBuilder.Allocate(ref this.AnimationClips, layer.AnimationClip.Length);
                for (int i = 0; i < layer.AnimationClip.Length; i++) blobClipArray[i].MakeBlob(layer.AnimationClip[i], blobBuilder);
            }
            if (layer.AnimatorStates.Length > 0)
            {
                var blobStateArray = blobBuilder.Allocate(ref this.AnimatorStates, layer.AnimatorStates.Length);
                for (int i = 0; i < layer.AnimatorStates.Length; i++) blobStateArray[i].MakeBlob(layer.AnimatorStates[i], blobBuilder);
            }
        }

        public AnimatorStateBlob GetStateBlob(int hashCode)
        {
            for (int i = 0; i < AnimatorStates.Length; i++)
            {
                if (AnimatorStates[i].Equals(hashCode))
                {
                    return AnimatorStates[i];
                }
            }

            return GetStateBlob(DefaultStateHashCode);
        }
    }

    public struct AnimatorLayer : IDisposable
    {
        public FixedString128Bytes Name;

        public UnsafeList<AnimationClip> AnimationClip;
        public NativeArray<AnimatorState> AnimatorStates;
        public int CurrentStateHashCode;
        public int DefaultStateHashCode;

        public void Dispose()
        {
            foreach (var clip in AnimationClip) clip.Dispose();
            foreach (var s in AnimatorStates) s.Dispose();

            AnimationClip.Dispose();
            AnimatorStates.Dispose();
        }
    }
}