using Assets.Scripts.AgentSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableAgent : MonoBehaviour
{
    Transform SelectionMarker;

    #region UnityMethods

    private void OnMouseDown()
    {
        //Notify agent has been clicked
        AgentSelector.NotifyAgentSelected(this);
        Select();
        
    }

    private void Awake()
    {
        SelectionMarker = transform.Find("SelectionMarker");
    }

    private void OnEnable()
    {
        //Select();
    }

    private void OnDisable()
    {
        Deselect();
    }

    #endregion

    public void Select()
    {
        //visuals
        var mark = SelectionMarker.GetComponent<SpriteRenderer>();
        mark.enabled = true;
        
    }

    public void Deselect()
    {
        var mark = SelectionMarker.GetComponent<SpriteRenderer>();
        mark.enabled = false;
    }

}
