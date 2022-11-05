using UnityEngine;

namespace Assets.Scripts.ItemSystem
{
    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;

        public int Id { get => _id; }
        public string Name { get => _name; }
        public Sprite Sprite { get => _sprite; }
    }
}