using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public struct AnimationClipBlobReference : IComponentData
    {
        public BlobAssetReference<AnimationClip> Clip;
    }

    public struct AnimationClipBlob : IEquatable<int>
    {
        public FixedString128Bytes Name;
        public float Length;
        public float FrameRate;
        public WrapMode WrapMode;
        public bool Legacy;
        public Bounds Bounds;

        public BlobArray<AnimationCurveBlob> Curves;

        public int HashCode;
        public bool Equals(int hash) => hash == HashCode;
        public void MakeBlob(AnimationClip clip, BlobBuilder blobBuilder)
        {
            Name = clip.Name;
            Length = clip.Length;
            FrameRate = clip.FrameRate;
            WrapMode = clip.WrapMode;
            Legacy = clip.Legacy;
            Bounds = clip.Bounds;
            HashCode = clip.GetHashCode();

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

        public AnimationClip(UnityEngine.AnimationClip clip)
        {
            Name = clip.name;
            Length = clip.length;
            FrameRate = clip.frameRate;
            WrapMode = clip.wrapMode;
            Legacy = clip.legacy;
            Bounds = clip.localBounds;
                
            var binds = AnimationUtility.GetCurveBindings(clip);
            CurveAuthorings = new UnsafeList<AnimationCurve>(binds.Length, Allocator.Persistent);
            foreach (var bind in binds) CurveAuthorings.Add(new AnimationCurve(bind, clip));
        }

        public bool Equals(int hash) => hash == GetHashCode();
        public void Dispose()
        {
            foreach (var curve in CurveAuthorings) curve.Dispose();
            CurveAuthorings.Dispose();
        }
    }
}