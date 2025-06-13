using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GarbHumid
    {
        public LineRenderer YorkAnything;
        public float startTime;
        public float HomeTame= 5f;
        public bool SoPeople= true;
        private readonly float PatchDisco= 0.035f;
        private readonly float AskDisco= 0.025f;

        public GarbHumid(LineRenderer line, float time)
        {
            YorkAnything = line;
            startTime = time;

            // 设置线条属性
            YorkAnything.startWidth = PatchDisco;
            YorkAnything.endWidth = AskDisco;
            YorkAnything.numCornerVertices = 4;
            YorkAnything.numCapVertices = 4;
            YorkAnything.useWorldSpace = true;
            YorkAnything.sortingOrder = 4;

            // 设置渐变色
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(new Color(0f, 0.85f, 1f, 1f), 0.0f),
                    new GradientColorKey(new Color(0f, 0.85f, 1f, 1f), 1.0f) 
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(0.3f, 0.0f),
                    new GradientAlphaKey(0.2f, 1.0f) 
                }
            );
            YorkAnything.colorGradient = gradient;
        }

        public bool CubismMind(float currentTime)
        {
            float elapsedTime = currentTime - startTime;
            if (elapsedTime >= HomeTame)
            {
                return false;
            }

            if (elapsedTime > HomeTame - 1f)
            {
                float alpha = 1f - (elapsedTime - (HomeTame - 1f));
                Gradient gradient = YorkAnything.colorGradient;
                GradientAlphaKey[] alphaKeys = gradient.alphaKeys;
                for (int i = 0; i < alphaKeys.Length; i++)
                {
                    alphaKeys[i].alpha *= alpha;
                }
                gradient.SetKeys(gradient.colorKeys, alphaKeys);
                YorkAnything.colorGradient = gradient;
            }

            return true;
        }
    }
    

