using UnityEngine;


namespace Extensions
{
    public static partial class RectTransformExtensions
    {
        public static (Vector2[], Vector3) GetData(this RectTransform obj)
        {
            return (new Vector2[4]
            {
                obj.anchorMin,
                obj.anchorMax,
                obj.anchoredPosition,
                obj.sizeDelta
            }, obj.localScale);
        }

        public static void ApplyData(this RectTransform obj, (Vector2[], Vector3) data)
        {
            obj.anchorMin = data.Item1[0];
            obj.anchorMax = data.Item1[1];
            obj.anchoredPosition = data.Item1[2];
            obj.sizeDelta = data.Item1[3];
            obj.localScale = data.Item2;
        }
    }
}
