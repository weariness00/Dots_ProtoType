using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;

namespace Gpu_Animation.Scripts
{
    [CreateAssetMenu(fileName = "GPU", menuName = "Animation Instancing", order = 0)]
    public class GPUAnimationAsset : ScriptableObject
    {
        public string path = "GPU Animation/Animation Asset";
        [ReadOnly] public GameObject Model;
        public AnimationClip[] clips;
        public Shader shader;

        private List<Texture2D> _textures = new List<Texture2D>();
        private int width = 0;
        private int height = 0;

        public void Bake()
        {
            foreach (var clip in clips)
                height += (int)(clip.length * clip.frameRate);
            
            var meshRenderers = Model.GetComponentsInChildren<MeshRenderer>();
            if (meshRenderers.Length > 0)
            {
                var parentObject = new GameObject(name = Model.name);
                foreach (var meshRenderer in meshRenderers)
                {
                    var newGO = Instantiate(meshRenderer.gameObject, parentObject.transform, true);
                    var mesh = meshRenderer.GetComponent<MeshFilter>().sharedMesh;
                    var material = ConvertShader(meshRenderer.sharedMaterial);
                    MakeAnimationTexture(newGO, mesh, mesh.sharedMaterials[0]);
                }
                
                PrefabUtility.SaveAsPrefabAsset(parentObject, $"Assets/{path}/Obj_{Model.name}.prefab");
                DestroyImmediate(parentObject);
                return;
            }
            
            var skinnedMeshRenderers = Model.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRenderers.Length > 0)
            {
                var parentObject = new GameObject(name = Model.name);
                foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
                {
                    var newGO = Instantiate(skinnedMeshRenderer.gameObject, parentObject.transform, true);
                    var skinned = newGO.GetComponent<SkinnedMeshRenderer>();
                    MakeAnimationTexture(newGO, skinned.sharedMesh, skinned.sharedMaterials[0]);
                }
                
                PrefabUtility.SaveAsPrefabAsset(parentObject, $"Assets/{path}/Obj_{Model.name}.prefab");
                DestroyImmediate(parentObject);
                return;
            }
        }

        void MakeAnimationTexture(GameObject meshGameObject, Mesh mesh, Material material)
        {
            if (SystemInfo.maxTextureSize < mesh.vertexCount)
            {
                Debug.LogWarning($"{SystemInfo.maxTextureSize}보다 버텍스 갯수가 더 많습니다.\nMeshName : {mesh.name}\nVertextCount : {mesh.vertexCount}");
                return;
            }
            
            width = mesh.vertexCount;
            Camera camera = Camera.main;
            RenderTexture rt = new RenderTexture(width, height, 24);
            camera.targetTexture = rt;
            RenderTexture.active = rt;
            int sumFrame = 0;

            Texture2D frameTexture = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);

            foreach (AnimationClip clip in clips)
            {
                for (int frame = 0; frame < clip.length * clip.frameRate; frame += 1)
                {
                    clip.SampleAnimation(meshGameObject, frame);

                    // 프레임 렌더링
                    camera.Render();

                    // RenderTexture를 Texture2D로 변환
                    frameTexture.ReadPixels(new Rect(0, frame + sumFrame, rt.width, frame + sumFrame + 1), 0,frame + sumFrame);
                    frameTexture.Apply();

                    // Clear the active RenderTexture
                }

                sumFrame += (int)clip.length;
            }
            
            // material.SetTexture("_Animation_Texture", frameTexture);
            System.IO.File.WriteAllBytes($"Assets/{path}/RenderTexture_{Model.name}.png", frameTexture.EncodeToPNG());

            _textures.Add(frameTexture);
            material = ConvertShader(material);
            RenderTexture.active = null;
        }

        Mesh MakeMseh(Mesh mesh)
        {
            var newMesh = new Mesh();
            newMesh.vertices = mesh.vertices;
            newMesh.triangles = mesh.triangles;
            UnityEditor.AssetDatabase.CreateAsset(newMaterial, $"Assets/{path}/GPU_{material.name}.mat");
        }

        Material ConvertShader(Material material)
        {
            Material newMaterial = new Material(material);
            newMaterial.shader = shader;
            UnityEditor.AssetDatabase.CreateAsset(newMaterial, $"Assets/{path}/GPU_{material.name}.mat");

            return newMaterial;
        }
    }
}