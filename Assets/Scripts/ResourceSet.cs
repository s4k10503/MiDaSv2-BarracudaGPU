using UnityEngine;
using Unity.Barracuda;


[CreateAssetMenu(fileName = "MiDaSv2",
                 menuName = "ScriptableObjects/MiDaSv2 Resource Set")]
public sealed class ResourceSet : ScriptableObject
{
    public NNModel Model;
    public ComputeShader Preprocess;
    public ComputeShader Postprocess;
}