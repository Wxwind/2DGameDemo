using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FogFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Setting {
        public Material fogMaterial;
        public RenderPassEvent renderPassEvent;
        public string tag;
        public FilterMode filterMode;
    }

    public Setting setting;
   
    FogRenderPass m_FogRenderPass;

    public override void Create()
    {
        m_FogRenderPass = new FogRenderPass(setting.renderPassEvent,setting.fogMaterial,setting.tag,setting.filterMode);
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //获取相机的主纹理
        m_FogRenderPass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(m_FogRenderPass);
    }
}


