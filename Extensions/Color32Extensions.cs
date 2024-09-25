using UnityEngine;


namespace Extensions
{
    public static partial class Color32Extensions
    {
        private const int MAX_COLOR_BYTE = 255;
        private static byte GetRandomColorPart() => (byte)Random.Range(0, MAX_COLOR_BYTE + 1);

        public static Color32 RandomColor => new Color32(GetRandomColorPart(), GetRandomColorPart(), GetRandomColorPart(), MAX_COLOR_BYTE);

        public static Color32 SetAlpha(this Color32 c, byte a)
        {
            c.a = a;
            return c;
        }
        
        public static string ToHex(this Color32 c) => $"#{c.r:X2}{c.g:X2}{c.b:X2}{c.a:X2}";
    }
}
