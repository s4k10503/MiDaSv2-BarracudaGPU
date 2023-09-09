using UnityEngine;
using Unity.Barracuda;


[CreateAssetMenu(fileName = "MiDaSv2",
                 menuName = "ScriptableObjects/MiDaSv2 Resource Set")]
public sealed class ResourceSet : ScriptableObject
{
    public NNModel model;
    public ComputeShader preprocess;
    public ComputeShader postprocess;
}