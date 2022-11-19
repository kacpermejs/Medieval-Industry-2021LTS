using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.PlaceableObjectBehaviour
{
    public class Storage : MonoBehaviour, IUICreator
    {
        public string title => "Storage";

        public VisualElement CreateUIContent()
        {
            var label = new Label(title);

            return label;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

