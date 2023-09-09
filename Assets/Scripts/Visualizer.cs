using UnityEngine;
using UnityEngine.UI;
using MiDaSv2;
using Klak.TestTools;

public class Visualizer : MonoBehaviour
{
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] RawImage _previewSource = null;
    [SerializeField] RawImage _previewDepth = null;
    [SerializeField] ImageSource _source = null;

    private RenderTexture _renderTexture;
    RunInference _runInference;

    void Start()
    {
        _runInference = new RunInference(_resources);
        _renderTexture = new RenderTexture(_source.Texture.width, _source.Texture.height, 24);
    }

    void Update()
    {
        Graphics.Blit(_source.Texture, _renderTexture);
        _runInference.ProcessImage(_renderTexture);

        _previewSource.texture = _source.Texture;
        _previewDepth.texture = _runInference.DepthTexture;
    }

    private void OnDisable()
    {
        _runInference.Dispose();
        _renderTexture.Release();
    }
}
