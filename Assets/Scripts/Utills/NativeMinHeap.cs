// src: https://gist.github.com/andrew-raphael-lukasik/bc05310efcb1bcb2875a376494414f36
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

public interface INativeMinHeapComparer<I, V>
    where I : unmanaged
    where V : unmanaged
{
    int Compare(I lhs, I rhs, NativeSlice<V> comparables);
}

public struct NativeMinHeap<I, C, V> : System.IDisposable
    where I : unmanaged, System.IEquatable<I>
    where C : unmanaged, INativeMinHeapComparer<I, V>
    where V : unmanaged
{
    NativeList<I> _stack;
    C _comparer;
    [NativeDisableContainerSafetyRestriction]// oh boi, here comes trouble!
    NativeSlice<V> _comparables;

    public int Length => _stack.Length;
    public int Count => _stack.Length;
    public bool IsCreated => _stack.IsCreated;

    public NativeMinHeap(int capacity, Allocator allocator, NativeSlice<V> comparables)
    {
        this._stack = new NativeList<I>(capacity, allocator);
        this._comparables = comparables;
        this._comparer = default(C);
    }

    public void Push(I item)
    {
        _stack.Add(item);
        MinHeapifyUp(_stack.Length - 1);
    }
    public I Pop()
    {
        I removedItem = _stack[0];
        _stack.RemoveAtSwapBack(0);
        MinHeapifyDown(0);
        return removedItem;
    }

    public I Peek() => _stack[0];
    public void Clear() => _stack.Clear();

    void MinHeapifyUp(int childIndex)
    {
        if (childIndex == 0) return;
        int parentIndex = (childIndex - 1) / 2;
        I childVal = _stack[childIndex];
        I parentVal = _stack[parentIndex];
        if (_comparer.Compare(childVal, parentVal, _comparables) < 0)
        {
            // swap the parent and the child
            _stack[childIndex] = parentVal;
            _stack[parentIndex] = childVal;
            MinHeapifyUp(parentIndex);
        }
    }

    void MinHeapifyDown(int index)
    {
        int leftChildIndex = index * 2 + 1;
        int rightChildIndex = index * 2 + 2;
        int smallestItemIndex = index;// The index of the parent
        if (
            leftChildIndex <= this._stack.Length - 1
            && _comparer.Compare(_stack[leftChildIndex], _stack[smallestItemIndex], _comparables) < 0)
        {
            smallestItemIndex = leftChildIndex;
        }
        if (
            rightChildIndex <= this._stack.Length - 1
            && _comparer.Compare(_stack[rightChildIndex], _stack[smallestItemIndex], _comparables) < 0)
        {
            smallestItemIndex = rightChildIndex;
        }
        if (smallestItemIndex != index)
        {
            // swap the parent with the smallest of the child items
            I temp = _stack[index];
            _stack[index] = _stack[smallestItemIndex];
            _stack[smallestItemIndex] = temp;
            MinHeapifyDown(smallestItemIndex);
        }
    }

    public int Parent(int key) => (key - 1) / 2;
    public int Left(int key) => 2 * key + 1;
    public int Right(int key) => 2 * key + 2;
    public NativeArray<I> AsArray() => _stack.AsArray();
    public void Dispose() => _stack.Dispose();

    [System.Obsolete("use `heap.AsArray().Contains(value)` instead", true)]
    public bool Contains(I value)
    {
        // foreach( var next in _stack.AsArray() )
        // 	if( next==value )// this wont compile
        // 		return true;
        return false;
    }

}