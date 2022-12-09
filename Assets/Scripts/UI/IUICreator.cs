using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public interface IUICreator
    {
        string title { get; }
        VisualElement CreateUIContent();

        void RegisterCallbacks();

        void UnregisterCallbacks();
    }
}


