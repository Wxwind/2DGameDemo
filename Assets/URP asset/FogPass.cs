using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

class FogRenderPass : ScriptableRenderPass
{
    private string m_commandBufferTag;
    private Material m_fogMaterial;
    private RenderTargetIdentifier m_cameraColorIdentifier;
    private FilterMode filterMode;

    private RenderTargetHandle m_temp;
    public FogRenderPass(RenderPassEvent evt, Material material, string tag,FilterMode filterMode)
    {
        renderPassEvent = evt;
        m_commandBufferTag = tag;
        this.filterMode = filterMode;
        if (material == null)
        {
            Debug.LogWarningFormat("urp pp's Fog Material is missing,{0} wouldn't be executed", GetType().Name);
            return;
        }
        m_fogMaterial = material;
        m_temp.Init("temp");
    }
    public void Setup(RenderTargetIdentifier cameraColorTarget)
    {
        this.m_cameraColorIdentifier = cameraColorTarget;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
    }


    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (!renderingData.cameraData.postProcessEnabled)
        {
            return;
        }
        var cmd = CommandBufferPool.Get(m_commandBufferTag);
        RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        cmd.GetTemporaryRT(m_temp.id, cameraTargetDescriptor,filterMode);
        cmd.Blit(m_cameraColorIdentifier, m_temp.Identifier(), m_fogMaterial);
        cmd.Blit(m_temp.Identifier(), m_cameraColorIdentifier);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
        cmd.ReleaseTemporaryRT(m_temp.id);
    }

    /// Cleanup any allocated resources that were created during the execution of this render pass.
    public override void FrameCleanup(CommandBuffer cmd)
    {
    }
}
