using UnityEngine;

namespace LeafRand
{
    /// <summary>
    /// Scriptable object holding a weighted list of gameobjects.
    /// Allows for inspector reusable WeightedLists.
    /// </summary>
    [CreateAssetMenu(fileName = "newWeightedObjectList", menuName = "Weighted Object List")]
    public class WeightedObjectList : ScriptableObject
    {
        [SerializeField] WeightedList<GameObject> weightedObjects = new();

        // Returns a copy of WeightedObjects
        // We do not want anyone to ever take a ref to weighted objects and edit it
        // as those changes would save and we could lose fine tuned objects and weights
        public WeightedList<GameObject> GetWeightedListCopy() => weightedObjects.Clone();
    }
}