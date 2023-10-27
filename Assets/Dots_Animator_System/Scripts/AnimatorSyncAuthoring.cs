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

    // public struct Paramater
    // {
    //     [FieldOffset(0)]
    //     public float FloatValue;
    //     [FieldOffset(0)]
    //     public int IntValue;
    //     [FieldOffset(0)]
    //     public bool BoolValue;
    //
    //     public static implicit operator Paramater(float f) => new Paramater() { floatValue = f };
    //     public static implicit operator Paramater(int i) => new Paramater() { intValue = i };
    //     public static implicit operator Paramater(bool b) => new Paramater() { boolValue = b };
    // }
    
    public struct AnimatorSync : IComponentData
    {
        public FixedString64Bytes Name;
        public AnimatorSyncAuthoring AnimatorSyncAuthoring;
    }
    
    public struct AnimatorSyncAuthoring : IDisposable
    {
        public UnsafeList<AnimatorLayer> LayerAuthorings;
    
        public void Dispose()
        {
            foreach (var layer in LayerAuthorings) layer.Dispose();
            LayerAuthorings.Dispose();
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
