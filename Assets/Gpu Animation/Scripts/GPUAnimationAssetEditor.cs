using UnityEditor;
using UnityEngine;

namespace Gpu_Animation.Scripts
{
    [CustomEditor(typeof(GPUAnimationAsset))]
    public class GPUAnimationAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var s = (GPUAnimationAsset)target;
            if (GUILayout.Button("GPU Animation Bake"))
            {
                s.Bake();
            }
        }
    }
}
