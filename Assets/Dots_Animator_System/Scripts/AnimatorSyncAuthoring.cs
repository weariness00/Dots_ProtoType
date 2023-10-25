using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public struct AnimatorSync : IComponentData
    {
        public AnimatorSyncAuthoring AnimatorSyncAuthoring;
    }
    
    public struct AnimatorSyncAuthoring : IDisposable
    {
        public UnsafeList<AnimatorSyncLayerAuthoring> LayerAuthorings;

        public void Dispose()
        {
            foreach (var layer in LayerAuthorings) layer.Dispose();
            LayerAuthorings.Dispose();
        }
    }
    
    public struct AnimatorSyncLayerAuthoring : IDisposable
    {
        public FixedString64Bytes Name;
        
        public UnsafeList<AnimationClipAuthoring> AnimationClipAuthorings;

        public void Dispose()
        {
            foreach (var clip in AnimationClipAuthorings) clip.Dispose();
            AnimationClipAuthorings.Dispose();
        }
    }
    
    public struct AnimationClipAuthoring : IDisposable
    {
        public FixedString64Bytes Name;
        public float Length;
        public float FrameRate;
        public WrapMode WrapMode;
        public bool Legacy;
        public Bounds Bounds;
    
        public UnsafeList<AnimationCurveAuthoring> CurveAuthorings;

        public void Dispose()
        {
            foreach (var curve in CurveAuthorings) curve.Dispose();
            CurveAuthorings.Dispose();
        }
    }
    
    public struct AnimationCurveAuthoring : IDisposable
    {
        public FixedString64Bytes PropertyName;
    
        public UnsafeList<Keyframe> KeyFrames;

        public void Dispose()
        {
            KeyFrames.Dispose();
        }
    }
}
