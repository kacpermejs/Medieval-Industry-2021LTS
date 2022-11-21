public interface ISelect
{
    bool IsSelected { get; }
    void Select();
    void Deselect();
}
