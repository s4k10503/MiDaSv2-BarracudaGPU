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
    }
}
