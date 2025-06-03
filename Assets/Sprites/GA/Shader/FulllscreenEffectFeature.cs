using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FullscreenEffectFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class FullscreenEffectSettings
    {
        public Material material;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public FullscreenEffectSettings settings = new FullscreenEffectSettings();

    class FullscreenEffectPass : ScriptableRenderPass
    {
        private Material material;
        private RTHandle tempRT;

        public FullscreenEffectPass(Material material, RenderPassEvent passEvent)
        {
            this.material = material;
            this.renderPassEvent = passEvent;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Get descriptor for the camera target
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;

            // Allocate a temporary RTHandle (replaces RenderTargetHandle)
            RenderingUtils.ReAllocateIfNeeded(ref tempRT, descriptor, name: "_TempFullscreenEffectTex");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("Fullscreen Effect");

            RTHandle cameraColor = renderingData.cameraData.renderer.cameraColorTargetHandle;

            // Copy camera color to temp RT
            Blit(cmd, cameraColor, tempRT);

            // Set global texture for shader
            cmd.SetGlobalTexture("_MainTex", tempRT);

            // Blit effect back to camera color
            Blit(cmd, tempRT, cameraColor, material);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            tempRT?.Release();
        }
    }

    FullscreenEffectPass pass;

    public override void Create()
    {
        pass = new FullscreenEffectPass(settings.material, settings.renderPassEvent);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}