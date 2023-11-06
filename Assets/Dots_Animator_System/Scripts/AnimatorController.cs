using System;
using System.Runtime.InteropServices;
using Rukhanka;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public class AnimatorManaged : IComponentData
    {
        public Animator Animator;
    }

    public struct AnimatorTag : IComponentData {}

    public struct AnimatorControllerBlobAssetReference : IBufferElementData
    {
        public BlobAssetReference<AnimatorControllerBlob> AnimatorReference;
        public int LayerIndex;
        public FixedString128Bytes LayerName;
        public float LayerWeight;

        public AnimatorStateBlob CurrenStateBlob;
    }

    public struct AnimatorControllerBlob
    {
        public FixedString64Bytes Name;
        public BlobArray<AnimatorLayerBlob> Layers;
        public BlobArray<AnimationClipBlob> AnimationClipArray;
        public BlobArray<BoneInfoBlob> BoneInfoArray;
        
        public void MakeBlob(DynamicBuffer<BoneInfo> boneInfoArray, AnimatorController animatorController, BlobBuilder blobBuilder)
        {
            Name = animatorController.Name;

            var layerLength = animatorController.LayerAuthorings.Length;
            if (layerLength > 0)
            {
                var blobLayers = blobBuilder.Allocate(ref this.Layers, layerLength);
                for (int i = 0; i < layerLength; i++) blobLayers[i].MakeBlob(animatorController.LayerAuthorings[i], blobBuilder);
            }

            if (boneInfoArray.Length > 0)
            {
                var blobBoneInfoArray = blobBuilder.Allocate(ref this.BoneInfoArray, boneInfoArray.Length);
                for (int i = 0; i < boneInfoArray.Length; i++) blobBoneInfoArray[i].MakeBlob(boneInfoArray[i]);
            }

            if (animatorController.AnimationClips.Length > 0)
            {
                var blobAnimationClipArray = blobBuilder.Allocate(ref this.AnimationClipArray, animatorController.AnimationClips.Length);
                for (int i = 0; i < animatorController.AnimationClips.Length; i++) blobAnimationClipArray[i].MakeBlob(animatorController.AnimationClips[i], blobBuilder);
            }
        }
    }

    public struct AnimatorController : IComponentData, IDisposable
    {
        public FixedString64Bytes Name;
        public UnsafeList<AnimatorLayer> LayerAuthorings;
        public UnsafeList<AnimationClip> AnimationClips;
        
        public void Dispose()
        {
            foreach (var layer in LayerAuthorings) layer.Dispose();
            foreach (var clip in AnimationClips) clip.Dispose();
            LayerAuthorings.Dispose();
            AnimationClips.Dispose();
        }
    }
}