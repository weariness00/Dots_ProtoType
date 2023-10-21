using System;
using Unity.Collections;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public struct AnimatorSyncAuthoring : ISharedComponentData
    {
        public NativeArray<AnimatorSyncLayerAuthoring> LayerAuthorings;
    }
    
    public struct AnimatorSyncLayerAuthoring
    {
        public FixedString64Bytes Name;
        
        public NativeArray<AnimationClipAuthoring> AnimationClipAuthorings;
    }
    
    public struct AnimationClipAuthoring
    {
        public FixedString64Bytes Name;
        public float Length;
        public float FrameRate;
        public WrapMode WrapMode;
        public bool Legacy;
        public Bounds Bounds;
    
        public NativeArray<AnimationCurveAuthoring> CurveAuthorings;
    }
    
    public struct AnimationCurveAuthoring
    {
        public FixedString64Bytes PropertyName;
    
        public NativeArray<Keyframe> KeyFrames;
    }
}
