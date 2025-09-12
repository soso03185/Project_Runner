using UnityEngine;

public class SkyboxControl
{
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.4f);
    }

}
