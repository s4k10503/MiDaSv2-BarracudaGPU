using UnityEngine;
using UnityEngine.UI;
using MiDaSv2;
using Klak.TestTools;

public class Visualizer : MonoBehaviour
{
    [SerializeField] private ResourceSet _resources = null;
    [SerializeField] private RawImage _previewSource = null;
    [SerializeField] private RawImage _previewDepth = null;
    [SerializeField] private ImageSource _source = null;

    private RenderTexture _renderTexture;
    private RunInference _runInference;

    private void Start()
    {
        _runInference = new RunInference(_resources);
        _renderTexture = new RenderTexture(_source.Texture.width, _source.Texture.height, 24);
        _renderTexture.Create();
    }

    private void Update()
    {
        Graphics.Blit(_source.Texture, _renderTexture);
        _runInference.ProcessImage(_renderTexture);

        _previewSource.texture = _source.Texture;
        _previewDepth.texture = _runInference.DepthTexture;
    }

    private void OnDestroy()
    {
        if (_runInference != null)
        {
            _runInference.Dispose();
            _runInference = null;
        }

        if (_renderTexture != null)
        {
            _renderTexture.Release();
            Destroy(_renderTexture);
            _renderTexture = null;
        }
    }
}
