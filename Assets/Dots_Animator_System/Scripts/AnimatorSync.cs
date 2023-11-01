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
    // Bone 가져오기
    // Blob 에셋 사용해야됨

    public class AnimatorManaged : IComponentData
    {
        public Animator Animator;
    }

    public struct AnimatorSyncBlobAssetReference : IComponentData
    {
        public BlobAssetReference<AnimatorSyncBlob> Animator;
    }

    public struct AnimatorSyncBlob
    {
        public FixedString64Bytes Name;
        public BlobArray<AnimatorLayerBlob> Layers;

        public void MakeBlob(AnimatorSync animatorSync, BlobBuilder blobBuilder)
        {
            Name = animatorSync.Name;

            var layerLength = animatorSync.LayerAuthorings.Length;
            if (layerLength > 0)
            {
                var blobLayers = blobBuilder.Allocate(ref this.Layers, layerLength);
                for (int i = 0; i < layerLength; i++) blobLayers[i].MakeBlob(animatorSync.LayerAuthorings[i], blobBuilder);
            }
        }
    }

    public struct AnimatorSync : IComponentData, IDisposable
    {
        public FixedString64Bytes Name;
        public UnsafeList<AnimatorLayer> LayerAuthorings;

        public void Dispose()
        {
            foreach (var layer in LayerAuthorings) layer.Dispose();
            LayerAuthorings.Dispose();
        }
    }
}