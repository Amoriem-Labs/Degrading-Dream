
Shader "Custom/PlayerDistanceFog"
{
    Properties
    {
        _MainTex ("Source", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (0.025, 0.035, 0.06, 1)
        _ClearRadius ("Clear Radius", Float) = 10
        _FogEnd ("Fog End Distance", Float) = 30
        _FogStrength ("Fog Strength", Range(0, 1)) = 1
    }

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;

            float4 _MainTex_TexelSize;
            float4 _FogColor;
            float4 _PlayerPosition;

            float _ClearRadius;
            float _FogEnd;
            float _FogStrength;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;

                #if UNITY_UV_STARTS_AT_TOP
                if (_MainTex_TexelSize.y < 0)
                    uv.y = 1 - uv.y;
                #endif

                fixed4 sceneColor = tex2D(_MainTex, uv);

                float rawDepth =
                    SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);

                // Reconstruct the visible surface's world position.
                float4 viewPos = mul(
                    unity_CameraInvProjection,
                    float4(uv * 2 - 1, 1, 1)
                );

                viewPos.xyz /= viewPos.w;

                float eyeDepth = LinearEyeDepth(rawDepth);

                viewPos.xyz *= eyeDepth / -viewPos.z;

                float3 worldPos = mul(
                    unity_CameraToWorld,
                    float4(viewPos.xyz, 1)
                ).xyz;

                // Measure horizontal distance from the player.
                float2 playerXZ = _PlayerPosition.xz;
                float2 surfaceXZ = worldPos.xz;

                float distanceFromPlayer =
                    distance(playerXZ, surfaceXZ);

                // Calculate the fog transition.
                float fogAmount = smoothstep(
                    _ClearRadius,
                    max(_FogEnd, _ClearRadius + 0.01),
                    distanceFromPlayer
                );

                fogAmount *= _FogStrength;

                // Blend the scene into the fog color.
                return lerp(
                    sceneColor,
                    _FogColor,
                    fogAmount
                );
            }

            ENDCG
        }
    }
}
