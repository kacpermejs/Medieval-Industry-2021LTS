using UnityEngine.UIElements;

namespace UI
{
    public interface IUICreator
    {
        string Title { get; }
        VisualElement CreateUIContent();

        void RegisterCallbacks();

        void UnregisterCallbacks();
    }
}


