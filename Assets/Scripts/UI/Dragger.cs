using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Dragger : MouseManipulator
{
    private Vector2 startPos;
    public bool IsActive { get; private set; }

    public Dragger()
    {
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        IsActive = false;
    }
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }
    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }
    private void OnMouseDown(MouseDownEvent evt)
    {
        if (CanStartManipulation(evt))
        {
            startPos = evt.localMousePosition;
            IsActive = true;
            target.CaptureMouse();
            evt.StopPropagation();
        }
    }

    private void OnMouseMove(MouseMoveEvent evt)
    {

        if (!IsActive || !target.HasMouseCapture())
            return;

        Vector2 diff = evt.localMousePosition - startPos;

        target.style.left = target.layout.x + diff.x;
        target.style.top = target.layout.y + diff.y;

        evt.StopPropagation();
    }

    private void OnMouseUp(MouseUpEvent evt)
    {
        if ( !IsActive || !target.HasMouseCapture() || !CanStartManipulation(evt) )
            return;
        target.ReleaseMouse();
        IsActive = false;

        evt.StopPropagation();
    }


}
