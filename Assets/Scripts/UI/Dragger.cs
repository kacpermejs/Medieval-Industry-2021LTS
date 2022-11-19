using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dragger : MouseManipulator
{
    #region Init
    private Vector2 m_Start;
    protected bool m_Active;

    public List<VisualElement> TargetElements = new List<VisualElement>();

    public Dragger()
    {
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        m_Active = false;
    }
    #endregion

    #region Registrations
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
    #endregion

    #region OnMouseDown
    protected void OnMouseDown(MouseDownEvent e)
    {
        if (e.propagationPhase == PropagationPhase.AtTarget || TargetElements.Contains(e.target as VisualElement))
        {
            if (m_Active)
            {
                e.StopImmediatePropagation();
                return;
            }

            if (CanStartManipulation(e))
            {
                m_Start = e.localMousePosition;

                m_Active = true;
                target.CaptureMouse();
                e.StopPropagation();
            }
        }
        
    }
    #endregion

    #region OnMouseMove
    protected void OnMouseMove(MouseMoveEvent e)
    {
        if (!m_Active || !target.HasMouseCapture())
            return;

        Vector2 diff = e.localMousePosition - m_Start;

        target.style.left = target.layout.x + diff.x;
        target.style.top = target.layout.y + diff.y;

        e.StopPropagation();
    }
    #endregion

    #region OnMouseUp
    protected void OnMouseUp(MouseUpEvent e)
    {
        if (!m_Active || !target.HasMouseCapture() || !CanStopManipulation(e))
            return;

        m_Active = false;
        target.ReleaseMouse();
        e.StopPropagation();
    }
    #endregion
}










/*public class Dragger : MouseManipulator
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
        target.RegisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.NoTrickleDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove, TrickleDown.NoTrickleDown);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp, TrickleDown.NoTrickleDown);
    }
    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.NoTrickleDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove, TrickleDown.NoTrickleDown);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp, TrickleDown.NoTrickleDown);
    }
    private void OnMouseDown(MouseDownEvent evt)
    {
        if (CanStartManipulation(evt))
        {
            startPos = evt.localMousePosition;
            IsActive = true;
            //target.CaptureMouse();

            Debug.Log("DraggerDown: " + evt.propagationPhase);
        }
    }

    private void OnMouseMove(MouseMoveEvent evt)
    {

        if (!IsActive *//*|| !target.HasMouseCapture()*//*)
            return;

        Vector2 diff = evt.localMousePosition - startPos;

        target.style.left = target.layout.x + diff.x;
        target.style.top = target.layout.y + diff.y;

        
    }

    private void OnMouseUp(MouseUpEvent evt)
    {
        if ( !IsActive || *//*!target.HasMouseCapture() ||*//* !CanStartManipulation(evt) )
            return;
        target.ReleaseMouse();
        IsActive = false;
        Debug.Log("DraggerUp: " + evt.propagationPhase);
    }


}*/
