using UnityEngine;

namespace MiDaSv2
{
    public static class Utils
    {
        public static RenderTexture CreateRenderTexture(int width, int height)
        {
            var renderTexture = new RenderTexture(width, height, 0)
            {
                enableRandomWrite = true
            };
            renderTexture.Create();
            return renderTexture;
        }

        public struct ThreadSize
        {
            public uint X { get; }
            public uint Y { get; }
            public uint Z { get; }

            public ThreadSize(uint x, uint y, uint z) : this()
            {
                X = x;
                Y = y;
                Z = z;
            }
        }
    }
}
