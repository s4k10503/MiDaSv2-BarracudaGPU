using Unity.Barracuda;
using UnityEngine;
using System;

namespace MiDaSv2
{
    public class RunInference : IDisposable
    {
        private IWorker worker;
        private ComputeBuffer inputBuffer;
        private ComputeBuffer outputBuffer;
        private ResourceSet resources;
        public RenderTexture DepthTexture { get; private set; }
        ThreadSize _threadSize;

        private const int inputWidth = 256;
        private const int inputHeight = 256;
        private const int inputChannels = 3;

        public RunInference(ResourceSet resourceSet)
        {
            Initialize(resourceSet);
        }

        private void Initialize(ResourceSet resourceSet)
        {
            resources = resourceSet;

            var model = ModelLoader.Load(resources.model);
            worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);

            inputBuffer = new ComputeBuffer(1 * inputWidth * inputHeight * inputChannels, sizeof(float));
            outputBuffer = new ComputeBuffer(1 * inputWidth * inputHeight, sizeof(float));

            DepthTexture = Utils.CreateRenderTexture(inputWidth, inputHeight);
        }

        public void ProcessImage(RenderTexture sourceTexture)
        {
            // Preprocessing
            var preprocess = resources.preprocess;
            preprocess.GetKernelThreadGroupSizes(0, out _threadSize.x, out _threadSize.y, out _threadSize.z);
            preprocess.SetTexture(0, "InputTexture", sourceTexture);
            preprocess.SetBuffer(0, "OutputTensor", inputBuffer);
            preprocess.SetInt("Size", inputWidth);
            preprocess.Dispatch(0, inputWidth / (int)_threadSize.x, inputHeight / (int)_threadSize.y, (int)_threadSize.z);

            // Inference
            using (var tensor = new Tensor(1, inputWidth, inputHeight, inputChannels, inputBuffer))
            {
                worker.Execute(tensor);
            }

            var output = worker.PeekOutput();
            outputBuffer.SetData(output.AsFloats());

            // Postprocessing
            var postprocess = resources.postprocess;
            postprocess.GetKernelThreadGroupSizes(0, out _threadSize.x, out _threadSize.y, out _threadSize.z);
            postprocess.SetBuffer(0, "InputTensor", outputBuffer);
            postprocess.SetTexture(0, "OutputTexture", DepthTexture);
            postprocess.Dispatch(0, inputWidth / (int)_threadSize.x, inputHeight / (int)_threadSize.y, (int)_threadSize.z);
        }

        public void Dispose()
        {
            worker.Dispose();
            inputBuffer.Dispose();
            outputBuffer.Dispose();

            if (DepthTexture != null)
            {
                DepthTexture.Release();
                DepthTexture = null;
            }
        }
    }
}
