using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public struct AnimationCurveBlobReference : IComponentData
    {
        public BlobAssetReference<AnimationCurveBlob> Curve;
    }

    public struct AnimationCurveBlob
    {
        public FixedString64Bytes PropertyName;

        public BlobArray<Keyframe> KeyFrames;

        public AnimationCurveBlob(AnimationCurve curve, BlobBuilder blobBuilder)
        {
            this = blobBuilder.ConstructRoot<AnimationCurveBlob>();
            PropertyName = curve.PropertyName;

            var keyFramesLength = curve.KeyFrames.Length;
            if (keyFramesLength > 0)
            {
                var blobKeyFrames = blobBuilder.Allocate(ref this.KeyFrames, keyFramesLength);
                for (int i = 0; i < keyFramesLength; i++) blobKeyFrames[i] = curve.KeyFrames[i];
            }
        }
    }

    public struct AnimationCurve : IDisposable
    {
        public FixedString64Bytes PropertyName;
        public Entity Bone;

        public UnsafeList<Keyframe> KeyFrames;

        public void Dispose()
        {
            KeyFrames.Dispose();
        }
    }
}