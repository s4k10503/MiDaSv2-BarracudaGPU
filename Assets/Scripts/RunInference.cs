using Unity.Barracuda;
using UnityEngine;
using System;

namespace MiDaSv2
{
    public class RunInference : IDisposable
    {
        public RenderTexture DepthTexture { get; private set; }

        private IWorker _worker;
        private ComputeBuffer _inputBuffer;
        private ComputeBuffer _outputBuffer;
        private ResourceSet _resources;
        private Utils.ThreadSize _threadSize;

        private const int _inputWidth = 256;
        private const int _inputHeight = 256;
        private const int _inputChannels = 3;

        public RunInference(ResourceSet resourceSet)
        {
            Initialize(resourceSet);
        }

        private void Initialize(ResourceSet resourceSet)
        {
            _resources = resourceSet;

            var model = ModelLoader.Load(_resources.Model);
            _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);

            _inputBuffer = new ComputeBuffer(1 * _inputWidth * _inputHeight * _inputChannels, sizeof(float));
            _outputBuffer = new ComputeBuffer(1 * _inputWidth * _inputHeight, sizeof(float));

            DepthTexture = Utils.CreateRenderTexture(_inputWidth, _inputHeight);
        }

        private Utils.ThreadSize GetThreadSize(ComputeShader shader)
        {
            shader.GetKernelThreadGroupSizes(0, out uint x, out uint y, out uint z);
            return new Utils.ThreadSize(x, y, z);
        }

        public void ProcessImage(RenderTexture sourceTexture)
        {
            PreprocessImage(sourceTexture);
            PerformInference();
            PostprocessImage();
        }

        private void PreprocessImage(RenderTexture sourceTexture)
        {
            var preprocess = _resources.Preprocess;
            _threadSize = GetThreadSize(preprocess);
            preprocess.SetTexture(0, "InputTexture", sourceTexture);
            preprocess.SetBuffer(0, "OutputTensor", _inputBuffer);
            preprocess.SetInt("Size", _inputWidth);
            preprocess.Dispatch(0, _inputWidth / (int)_threadSize.X, _inputHeight / (int)_threadSize.Y, (int)_threadSize.Z);
        }

        private void PerformInference()
        {
            using (var tensor = new Tensor(1, _inputWidth, _inputHeight, _inputChannels, _inputBuffer))
            {
                _worker.Execute(tensor);
            }

            var output = _worker.PeekOutput();
            _outputBuffer.SetData(output.AsFloats());
        }

        private void PostprocessImage()
        {
            var postprocess = _resources.Postprocess;
            _threadSize = GetThreadSize(postprocess);
            postprocess.SetBuffer(0, "InputTensor", _outputBuffer);
            postprocess.SetTexture(0, "OutputTexture", DepthTexture);
            postprocess.Dispatch(0, _inputWidth / (int)_threadSize.X, _inputHeight / (int)_threadSize.Y, (int)_threadSize.Z);
        }

        public void Dispose()
        {
            _worker?.Dispose();
            _inputBuffer?.Dispose();
            _outputBuffer?.Dispose();

            if (DepthTexture != null)
            {
                DepthTexture.Release();
                DepthTexture = null;
            }
        }
    }
}
