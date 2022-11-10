using Assets.Scripts.ItemSystem;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.PlaceableObjectBehaviour
{
    public class Resource : MonoBehaviour
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _itemAmount;
        [SerializeField] private bool _renewable;

        public UnityEvent OnDepleted = new UnityEvent();
        public UnityEvent OnRenewed = new UnityEvent();

        public Item Item { get => _item; }
        public int ItemAmount { get => _itemAmount; }


        public void Renew(int amount)
        {
            if (_renewable)
            {
                _itemAmount += amount;
                OnRenewed?.Invoke();
            }
        }

        public void Consume(int amount)
        {
            if (amount <= _itemAmount)
            {
                _itemAmount -= amount;
            }
            else
            {
                _itemAmount = 0;
                OnDepleted?.Invoke();
            }
        }
    }
}

