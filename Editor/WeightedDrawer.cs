using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using LeafRand.Collections;

namespace LeafRand.Editor
{
    [CustomPropertyDrawer(typeof(Weighted<>))]
    public class WeightedDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            bool isInCollection = false;
            if (property.isArray) isInCollection = true;

            VisualElement root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;

            root.Add(new Label(isInCollection ? "Element 0" : property.displayName) { style = { width = 70, flexGrow = 0.5f, flexShrink = 0 } });
            root.Add(new PropertyField(property.FindPropertyRelative("item")) { label = "", style = { width = 100, flexGrow = 1, flexShrink = 0 } });
            root.Add(new Label("Weight") { style = { marginLeft = 15 } });


            var weightProp = property.FindPropertyRelative("weight");
            var weightField = new FloatField() { style = { width = 50, marginRight = 5 } };
            weightField.value = weightProp.floatValue;
            weightField.TrackPropertyValue(weightProp, (v) => { weightField.value = v.floatValue; });
            weightField.RegisterCallback<BlurEvent>(evt => ForcePositiveWeight());
            void ForcePositiveWeight() { weightField.value = Mathf.Max(0, weightField.value); weightProp.floatValue = weightField.value; weightProp.serializedObject.ApplyModifiedProperties(); }

            root.Add(weightField);

            return root;
        }

        

    }
}