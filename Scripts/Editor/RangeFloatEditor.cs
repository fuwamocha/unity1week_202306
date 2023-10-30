using MochaLib.Cores;
using UnityEditor;
namespace MochaLib.Editor
{
    [CustomPropertyDrawer(typeof(MochaFloatRange))]
    public class RangeFloatEditor : RangeEditor
    {
        protected override void ApplyValue(SerializedProperty minProperty, SerializedProperty maxProperty)
        {
            if (maxProperty.floatValue < minProperty.floatValue) maxProperty.floatValue = minProperty.floatValue;
        }
    }
}
