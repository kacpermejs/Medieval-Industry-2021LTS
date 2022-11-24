public interface ISelectableAgent
{
    bool IsSelected { get; }
    void Select();
    void Deselect();
}
