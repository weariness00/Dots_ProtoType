using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Gpu_Animation.Scripts
{
    [CreateAssetMenu(fileName = "GPU", menuName = "Animation Instancing", order = 0)]
    public class GPUAnimationAsset : ScriptableObject
    {
        public string path = "GPU Animation/Animation Asset";
        [ReadOnly] public GameObject Model;
        public AnimationClip[] clips;
        public int TotalFrame = 0;
        public Shader shader;

        private List<Texture2D> _textures = new List<Texture2D>();

        private void OnValidate()
        {
            TotalFrame = 0;
            foreach (var clip in clips)
                TotalFrame += (int)(clip.frameRate);
        }

        public void Bake()
        {
            // 디렉토리가 존재하지 않으면 생성
            if (!Directory.Exists($"{Application.dataPath}/{path}/{Model.name}"))
                Directory.CreateDirectory($"{Application.dataPath}/{path}/{Model.name}");

            // var meshRenderers = Model.GetComponentsInChildren<MeshRenderer>();
            // if (meshRenderers.Length > 0)
            // {
            //     var parentObject = new GameObject(name = Model.name);
            //     foreach (var meshRenderer in meshRenderers)
            //     {
            //         var newGO = Instantiate(meshRenderer.gameObject, parentObject.transform, true);
            //         var mesh = meshRenderer.GetComponent<MeshFilter>().sharedMesh;
            //         var material = ConvertShader(meshRenderer.sharedMaterial);
            //         MakeAnimationTexture(newGO, mesh);
            //     }
            //     
            //     PrefabUtility.SaveAsPrefabAsset(parentObject, $"Assets/{path}/Obj_{Model.name}.prefab");
            //     DestroyImmediate(parentObject);
            //     return;
            // }

            var skinnedMeshRenderers = Model.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRenderers.Length > 0)
            {
                var parentObject = new GameObject(name = Model.name);
                foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
                {
                    var newGO = Instantiate(skinnedMeshRenderer.gameObject, parentObject.transform, true);
                    var skinned = newGO.GetComponent<SkinnedMeshRenderer>();
                    var material = ConvertShader(skinned.sharedMaterials[0]);
                    var mesh = MakeMesh(skinned.sharedMesh);
                    skinned.sharedMesh = mesh;
                    skinned.sharedMaterials[0] = material;

                    MakeAnimationTexture(newGO, skinned.sharedMesh);
                }

                PrefabUtility.SaveAsPrefabAsset(parentObject, $"Assets/{path}/{Model.name}/Obj_{Model.name}.prefab");
                DestroyImmediate(parentObject);
                return;
            }
        }

        void MakeAnimationTexture(GameObject meshGameObject, Mesh mesh)
        {
            if (SystemInfo.maxTextureSize < mesh.vertexCount)
            {
                Debug.LogWarning($"{SystemInfo.maxTextureSize}보다 버텍스 갯수가 더 많습니다.\nMeshName : {mesh.name}\nVertextCount : {mesh.vertexCount}");
                return;
            }

            Texture2D vertexTexture = new Texture2D(mesh.vertexCount, TotalFrame, TextureFormat.RGBA32, false);
            Texture2D uvTexture = new Texture2D(mesh.vertexCount, TotalFrame, TextureFormat.RGBA32, false);
            Texture2D normalTexture = new Texture2D(mesh.vertexCount, TotalFrame, TextureFormat.RGBA32, false);
            List<Color> vertexColors = new List<Color>();
            List<Color> uvColors = new List<Color>();
            List<Color> normalColors = new List<Color>();

            for (int clipCount = 0; clipCount < clips.Length; clipCount++)
            {
                float currentFrame = 0;
                var clip = clips[clipCount];

                for (int frame = 0; frame < clip.frameRate; frame++)
                {
                    clip.SampleAnimation(Model, currentFrame);
                    for (int i = 0; i < mesh.vertexCount; i++)
                    {
                        float3 vertex = mesh.vertices[i];
                        vertexColors.Add(new Color(vertex.x, vertex.y, vertex.z));
                    }

                    for (int i = 0; i < mesh.uv.Length; i++)
                    {
                        float2 uv = mesh.uv[i];
                        uvColors.Add(new Color(uv.x,uv.y, 1));
                    }

                    for (int i = 0; i < mesh.normals.Length; i++)
                    {
                        float3 normal = mesh.normals[i];
                        normalColors.Add(new Color(normal.x, normal.y, normal.z));
                    }

                    currentFrame += clip.length / clip.frameRate;
                }
            }

            vertexTexture.SetPixels(0, 0, mesh.vertexCount, TotalFrame, vertexColors.ToArray());
            uvTexture.SetPixels(0, 0, mesh.vertexCount, TotalFrame, uvColors.ToArray());
            normalTexture.SetPixels(0, 0, mesh.vertexCount, TotalFrame, normalColors.ToArray());
            vertexTexture.Apply();
            uvTexture.Apply();
            normalTexture.Apply();

            System.IO.File.WriteAllBytes($"Assets/{path}/{Model.name}/VertexTexture_{meshGameObject.name}.png", vertexTexture.EncodeToPNG());
            System.IO.File.WriteAllBytes($"Assets/{path}/{Model.name}/UVTexture_{meshGameObject.name}.png", uvTexture.EncodeToPNG());
            System.IO.File.WriteAllBytes($"Assets/{path}/{Model.name}/NormalTexture_{meshGameObject.name}.png", normalTexture.EncodeToPNG());
        }

        Mesh MakeMesh(Mesh mesh)
        {
            var newMesh = new Mesh();
            newMesh.vertices = mesh.vertices;
            newMesh.triangles = mesh.triangles;
            newMesh.uv = mesh.uv;
            newMesh.normals = mesh.normals;
            UnityEditor.AssetDatabase.CreateAsset(newMesh, $"Assets/{path}/{Model.name}/GPU_{mesh.name}.asset");
            AssetDatabase.SaveAssets();
            return newMesh;
        }

        Material ConvertShader(Material material)
        {
            Material newMaterial = new Material(material);
            newMaterial.shader = shader;
            UnityEditor.AssetDatabase.CreateAsset(newMaterial, $"Assets/{path}/{Model.name}/GPU_{material.name}.mat");
            return newMaterial;
        }
    }
}