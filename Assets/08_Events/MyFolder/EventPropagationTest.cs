using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EventPropagationTest : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;


        VisualElement first = root.Q("first");
        VisualElement second = root.Q("second");
        VisualElement third = root.Q("third");
        var win = new PopupWindow();
        
        win.text = "wintext";
        win.style.position = Position.Absolute;
        win.Add(new Label("tekst"));
        win.AddManipulator(new Dragger());
        
        root.Add(win);

        first.RegisterCallback<MouseDownEvent>(
            (evt) =>
            {
                Debug.Log("First: " + evt.propagationPhase);
            });
        first.RegisterCallback<MouseUpEvent>(
            (evt) =>
            {
                Debug.Log("FirstUp: " + evt.propagationPhase);

            });
        //=======================================================================
        second.RegisterCallback<MouseDownEvent>(
            (evt) =>
            {
                Debug.Log("Second: " + evt.propagationPhase);
            });
        second.RegisterCallback<MouseUpEvent>(
            (evt) =>
            {
                Debug.Log("SecondUp: " + evt.propagationPhase);
            });
        //=======================================================================

        third.RegisterCallback<MouseDownEvent>(
            (evt) =>
            {
                Debug.Log("Third: " + evt.propagationPhase);
            });
        third.RegisterCallback<MouseUpEvent>(
            (evt) =>
            {
                Debug.Log("ThirdUp: " + evt.propagationPhase);

            });
        third.RegisterCallback<ClickEvent>(
            (evt) =>
            {
                Debug.Log("ThirdClick: " + evt.propagationPhase);

            });
        first.AddManipulator(new Dragger());

    }
}
