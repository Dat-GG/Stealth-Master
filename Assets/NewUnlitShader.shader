Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _WaterShallow("_WaterShallow", Color) = (1, 1, 1, 1)
        _WaterDeep("_WaterDeep", Color) = (1, 1, 1, 1)
        _WaveColor("_WaveColor", Color) = (1, 1, 1, 1)
        _Gloss("Gloss", float) = 1
        _ShorelineTex ("_ShorelineTex", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct VertexInput
            {
                float4 vertex : POSITION;
                float3 normal: NORMAL;
                float2 uv0 : TEXCOORD0;
            };

            struct VertexOutput
            {
                float4 clipSpacePos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normal: TEXCOORD1;
                float3 worldPos: TEXCOORD2;
            };

            sampler2D _ShorelineTex;

            float4 _Color;
            float _Gloss;
            uniform float3 _MousePos;

            float3 _WaterShallow;
            float3 _WaterDeep;
            float3 _WaveColor;

            // Vertex Shader
            VertexOutput vert( VertexInput v)
            {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normal = v.normal;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.clipSpacePos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float InvLerp(float a, float b, float value)
            {
                return (value - a) / (b - a);
            }

            float3 MyLerp(float3 a, float3 b, float t) 
            {
                return t * b + (1 - t) * a;
            }

            float Posterize(float steps, float value)
            {
                return floor(value * steps) / steps;
            }

            float4 frag(VertexOutput o) : SV_Target
            {
                float shorelineMap = tex2D(_ShorelineTex, o.uv0).x;
                
                float waveSize = 0.04;
                
                float shape = shorelineMap;

                float waveAmp = (sin(shape / waveSize + _Time.y * 4) + 1) * 0.5;

                waveAmp *= shorelineMap;

                float3 waterColor = lerp(_WaterDeep, _WaterShallow, shorelineMap);

                float3 waterWithWaves = lerp(waterColor, _WaveColor, waveAmp);

                return float4(waterWithWaves, 0);

                return frac(_Time.y);






                float dist = distance( _MousePos, o.worldPos);
                
                float glow = saturate(1 - dist);
   
                float2 uv = o.uv0;

                float3 colorA = float3(0.1, 0.8, 1);
                float3 colorB = float3(1, 0.8, 0.1);
                //float t = smoothstep(0.25, 0.75, uv.y);
                float t = uv.y;

                t = Posterize(16, t);

                //return t;

                float3 blend = MyLerp(colorA, colorB, t);

                //return float4(blend, 0);


                float3 normal = normalize(o.normal);

                // return float4(o.worldPos, 1);

                //Lighting
                float3 LightDir = _WorldSpaceLightPos0.xyz;
                float3 lightColor = _LightColor0.rbg;

                //Direct diffuse light
                float lightFalloff = max(0, dot(LightDir, normal));
                //lightFalloff = Posterize(4, lightFalloff);
                float3 directionDiffuseLight = lightFalloff * lightColor;

                //AmbientLight
                float3 ambientLight = float3(0.1, 0.1, 0.1);

                //Direct specular light
                float3 camPos = _WorldSpaceCameraPos;
                float3 fragToCam = camPos - o.worldPos;
                float3 viewDir = normalize(fragToCam);
                float3 viewReflect = reflect(-viewDir, normal);
                float specularFalloff = max(0, dot(viewReflect, LightDir));
                //specularFalloff = Posterize(8, specularFalloff);

                //Modify gloss
                specularFalloff = pow(specularFalloff, _Gloss);

                float3 directSpecular = specularFalloff * lightColor;                

                //Composite
                float3 diffuseLight = ambientLight + directionDiffuseLight;
                float3 finalSurfaceColor = diffuseLight * _Color.rbg + directSpecular + glow; 


                return float4(finalSurfaceColor, 0 );
                //3h04
            }
            ENDCG
        }
    }
}
