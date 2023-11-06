using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Dots_Animator_System.Scripts
{
    public class AnimatorSyncMono : MonoBehaviour
    {
        
    }

    public class AnimatorSyncBaker : Baker<Animator>
    {
        public override void Bake(Animator authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            // AddComponentObject(entity, new AnimatorManaged(){Animator = authoring});
        }
    }
}