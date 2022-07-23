Shader "Custom/MapStencilBuffer"
{
    Properties {}

    SubShader
    {
        Tags
        {
            "Queue" = "Geometry-1"
            "RenderType" = "Opaque"
        }

        Pass
        {
            ZWrite On
            ColorMask 0
        }
    }
}
