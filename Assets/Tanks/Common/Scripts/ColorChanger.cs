using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Common.Scripts
{
    public static class ColorChanger
    {
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private  static MaterialPropertyBlock _propertyBlock = new MaterialPropertyBlock();

        private static void SetColor(IEnumerable<Renderer> renderers, Color color)
        {
            foreach (var renderer in renderers)
            {
                if (renderer == null) continue;
                var materialCount = renderer.sharedMaterials.Length;
                for (var i = 0; i < materialCount; i++)
                {
                    renderer.GetPropertyBlock(_propertyBlock, i);
                    _propertyBlock.SetColor(BaseColorId, color);
                    renderer.SetPropertyBlock(_propertyBlock, i);
                }
            }
        }

        public static void Change(GameObject target, Color targetColor,int level)
        {
            var settings = target.GetComponent<TankSettingsData>();
            switch (level)
            {
                case 0:
                    SetColor(settings.RenderersFirst, targetColor);
                    break;

                case 1:
                    SetColor(settings.RenderersSecond, targetColor);
                    break;

                case 2:
                    SetColor(settings.RenderersThird, targetColor);
                    break;
            }
        }
    }
}