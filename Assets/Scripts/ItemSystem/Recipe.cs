using UnityEngine;

namespace Assets.Scripts.ItemSystem
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "ScriptableObjects/Recipe")]
    public class Recipe : ScriptableObject
    {
        [field: SerializeField] public Item[] Input { get; private set; }
        [field: SerializeField] public int[] InputAmounts { get; private set; }
        [field: SerializeField] public Item Output { get; private set; }
        [field: SerializeField] public int OutputAmount { get; private set; }
        [field: SerializeField] public int TimeNeeded { get; private set; }
    }
}


