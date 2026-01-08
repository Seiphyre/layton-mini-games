using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VForge
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Returns a copy of the color with a modified alpha value.
        /// </summary>
        public static Color WithOpacity(this Color color, float opacity)
        {
            color.a = Mathf.Clamp01(opacity);
            return color;
        }
    }
}
