using Assets.Scripts.BuildingSystem;
using UnityEngine;

namespace Assets.Scripts.PlaceableObjectBehaviour
{
    public class StructureProperties : MonoBehaviour
    {

        private void OnMouseDown()//Request popup window
        {
            if (TryGetComponent<PlaceableObject>(out PlaceableObject mainObj))
            {
                if (TryGetComponent<Producer>(out Producer obj))
                {
                    //Add Tab
                    //AddTab();

                }




            }


            Debug.Log(this.name);
        }
    }
}


