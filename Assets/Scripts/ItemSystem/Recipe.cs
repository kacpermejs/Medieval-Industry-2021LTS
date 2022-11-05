using UnityEngine;

namespace Assets.Scripts.ItemSystem
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "ScriptableObjects/Recipe")]
    public class Recipe : ScriptableObject
    {
        [SerializeField] private Item[] _input;
        [SerializeField] private int _inputAmount;
        [SerializeField] private Item _output;
        [SerializeField] private int _outputAmount;
        [SerializeField] private int _timeNeeded;

        public Item[] Input { get => _input; }
        public int InputAmount { get => _inputAmount; }
        public Item Output { get => _output; }
        public int OutputAmount { get => _outputAmount; }
        public int TimeNeeded { get => _timeNeeded; }
    }
}


