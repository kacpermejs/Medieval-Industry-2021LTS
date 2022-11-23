using UnityEngine.UIElements;

namespace Assets.Scripts.PlaceableObjectBehaviour
{
    public interface IUICreator
    {
        string title { get; }
        VisualElement CreateUIContent();

        void RegisterCallbacks();

        void UnregisterCallbacks();
    }
}


