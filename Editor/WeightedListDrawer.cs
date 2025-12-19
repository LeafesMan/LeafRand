using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LeafEditor;

namespace LeafRand
{
    [CustomPropertyDrawer(typeof(WeightedList<>))]
    public class WeightedListDrawer : PropertyDrawer
    {

        // Ui toolkit implementation of the above
        // better alignment plus respects any editor Attributes applied to the element
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty items = property.FindPropertyRelative("items");
            SerializedProperty weights = property.FindPropertyRelative("weights");


            Button addButton = new() { text = "Add" };
            Button removeButton = new() { text = "Remove" };


            // Dummy List same size as WeightedList that ensures listView updates properly
            List<int> dummyList = new();
            for (int i = 0; i < items.arraySize; i++) dummyList.Add(i);

            ListView listView = new ListView(dummyList, -1, MakeItem, BindItem)
            {
                style =
            {
                borderBottomColor = Color.black,
            }
            };
            listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            // Support Reorderability
            listView.reorderable = true;
            listView.reorderMode = ListViewReorderMode.Animated;
            listView.itemIndexChanged += (a, b) =>
            {
                items.MoveArrayElement(a, b);
                weights.MoveArrayElement(a, b);
                property.serializedObject.ApplyModifiedProperties();
            };
            listView.TrackPropertyValue(property, UpdateListView); // Update List
            listView.selectedIndicesChanged += UpdateRemoveButton;
            listView.selectionType = SelectionType.Multiple;

            addButton.clicked += AddItem;
            removeButton.clicked += RemoveItem;
            removeButton.SetEnabled(false);


            // Declare, Populate, and return Root
            bool isScriptableObject = property.serializedObject.targetObject is ScriptableObject;
            VisualElement contents = isScriptableObject ? EditorUtil.GetContainer() : new();
            contents.Add(listView);
            contents.Add(addButton);
            contents.Add(removeButton);

            // Support Drag Drop of elements
            contents.RegisterCallback<DragPerformEvent>(evt => DragPerform(evt, property));
            contents.RegisterCallback<DragUpdatedEvent>(DragUpdated);

            return isScriptableObject ? contents : EditorUtil.GetFoldout(EditorUtil.GetLabel(property.displayName), contents, EditorUtil.GetUniqueID(property));

            void AddItem()
            {
                items.arraySize++;
                weights.arraySize++;

                property.serializedObject.ApplyModifiedProperties();
            }
            void RemoveItem()
            {
                List<int> selectedIndices = listView.selectedIndices.ToList();
                selectedIndices.Sort();

                for (int i = selectedIndices.Count - 1; i >= 0; i--)
                {
                    items.DeleteArrayElementAtIndex(selectedIndices[i]);
                    weights.DeleteArrayElementAtIndex(selectedIndices[i]);
                }
                property.serializedObject.ApplyModifiedProperties();

                listView.selectedIndex = -1;
            }
            void UpdateListView(SerializedProperty prop)
            {
                while (items.arraySize != dummyList.Count)
                {
                    if (items.arraySize < dummyList.Count) dummyList.RemoveAt(dummyList.Count - 1);
                    else dummyList.Add(0);
                }
                listView.Rebuild();
            }
            void UpdateRemoveButton(IEnumerable<int> ints)
            {
                bool oneSelected = false;

                foreach (var i in ints) if (i != -1) { oneSelected = true; break; }
                removeButton.SetEnabled(oneSelected);
            }
            VisualElement MakeItem()
            {
                VisualElement root = new VisualElement();
                root.style.flexDirection = FlexDirection.Row;
                root.style.alignItems = Align.Center;

                root.Add(new Label() { style = { width = 50, flexShrink = 0 } });
                root.Add(new PropertyField(property.FindPropertyRelative("element")) { label = "", style = { width = 100, flexGrow = 1, flexShrink = 0 } });
                root.Add(new Label("Weight") { style = { marginLeft = 5 } });
                root.Add(new PropertyField(property.FindPropertyRelative("weight")) { style = { width = 50, marginRight = 5 } });

                return root;
            }
            void BindItem(VisualElement element, int i)
            {
                Label label = element.ElementAt(0) as Label;
                label.text = "Item " + i;

                PropertyField itemField = element.ElementAt(1) as PropertyField;
                itemField.Unbind();
                itemField.BindProperty(items.GetArrayElementAtIndex(i));

                PropertyField weightField = element.ElementAt(3) as PropertyField;
                weightField.Unbind();
                weightField.BindProperty(weights.GetArrayElementAtIndex(i));
            }
        }

        private void DragUpdated(DragUpdatedEvent evt)
        {
            evt.StopImmediatePropagation();

            // If there is at least one identifier being dragged --> Accept
            // Otherwise --> Deny
            /*bool foundPrefab = false;
            foreach (var obj in DragAndDrop.objectReferences)
                if (obj.IsPrefabDefinition())
                {
                    foundPrefab = true;
                    break;
                }

            DragAndDrop.visualMode = foundPrefab ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.None;*/

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }
        private void DragPerform(DragPerformEvent evt, SerializedProperty prop)
        {   // Only Purge Duplicates after a drag and drop
            DragAndDrop.AcceptDrag();

            // Since the Property we pass in is cached our scriptable object can become stale
            // thus we update it to point to the latest instance
            //serializedObject.Update();

            // Go through all DraggedDropped Elements
            // - Skip non GameObjects
            // - Add rest
            int added = 0;
            SerializedProperty items = prop.FindPropertyRelative("items");
            SerializedProperty weights = prop.FindPropertyRelative("weights");
            foreach (var draggedObject in DragAndDrop.objectReferences)
            {
                // Dragged Object is a Prefab Gate
                /*if (!draggedObject.IsPrefabDefinition())
                {
                    Debug.LogWarning($"Skipping {draggedObject.name}, not a GameObject");
                    continue;
                }*/

                // Add to array
                items.arraySize++;
                weights.arraySize++;
                items.GetArrayElementAtIndex(items.arraySize - 1).objectReferenceValue = draggedObject as GameObject;

                Debug.Log("Added " + draggedObject.name);

                added++;
            }

            prop.serializedObject.ApplyModifiedProperties();
        }
    }
}