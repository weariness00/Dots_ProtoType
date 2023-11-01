using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public struct AnimationClipBlobReference : IComponentData
    {
        public BlobAssetReference<AnimationClip> Clip;
    }

    public struct AnimationClipBlob
    {
        public FixedString128Bytes Name;
        public float Length;
        public float FrameRate;
        public WrapMode WrapMode;
        public bool Legacy;
        public Bounds Bounds;

        public BlobArray<AnimationCurveBlob> Curves;

        public void MakeBlob(AnimationClip clip, BlobBuilder blobBuilder)
        {
            Name = clip.Name;
            Length = clip.Length;
            FrameRate = clip.FrameRate;
            WrapMode = clip.WrapMode;
            Legacy = clip.Legacy;
            Bounds = clip.Bounds;

            var curveLength = clip.CurveAuthorings.Length;
            if (curveLength > 0)
            {
                var blobCurveArray = blobBuilder.Allocate(ref this.Curves, curveLength);
                for (int i = 0; i < curveLength; i++) blobCurveArray[i].MakeBlob(clip.CurveAuthorings[i], blobBuilder);
            }
        }
    }

    public struct AnimationClip : IEquatable<int>, IDisposable
    {
        public FixedString128Bytes Name;
        public float Length;
        public float FrameRate;
        public WrapMode WrapMode;
        public bool Legacy;
        public Bounds Bounds;

        public UnsafeList<AnimationCurve> CurveAuthorings;

        public bool Equals(int hash) => hash == GetHashCode();
        public void Dispose()
        {
            foreach (var curve in CurveAuthorings) curve.Dispose();
            CurveAuthorings.Dispose();
        }
    }
}