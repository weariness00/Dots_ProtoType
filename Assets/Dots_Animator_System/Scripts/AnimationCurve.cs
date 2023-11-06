using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public struct AnimationCurveBlobReference : IComponentData
    {
        public BlobAssetReference<AnimationCurveBlob> Curve;
    }

    public struct AnimationCurveBlob
    {
        public FixedString128Bytes PropertyName;
        public Entity BoneEntity;

        public BlobArray<Keyframe> KeyFrames;

        public void MakeBlob(AnimationCurve curve, BlobBuilder blobBuilder)
        {
            PropertyName = curve.PropertyName;
            BoneEntity = curve.Bone;

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
        public FixedString128Bytes PropertyName;
        public Entity Bone;

        public UnsafeList<Keyframe> KeyFrames;
        
        public AnimationCurve(EditorCurveBinding bind, UnityEngine.AnimationClip clip)
        {
            UnityEngine.AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, bind);

            PropertyName = bind.propertyName;
            Bone = Entity.Null;

            KeyFrames = new UnsafeList<Keyframe>(curve.keys.Length, Allocator.Persistent);

            foreach (var key in curve.keys) KeyFrames.Add(key);
        }

        public void Dispose()
        {
            KeyFrames.Dispose();
        }
    }
}