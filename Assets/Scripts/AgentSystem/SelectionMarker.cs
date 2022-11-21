using Assets.Scripts.AgentSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMarker : MonoBehaviour
{
    private GameObject Marker;

    public bool IsSelected { get; private set; }

    #region UnityMethods

    /*private void OnMouseDown()
    {
        Select();
    }*/

    private void Awake()
    {
        Marker = transform.Find("SelectionMarker").gameObject;
    }

    private void OnEnable()
    {
        Deselect();
    }

    private void OnDisable()
    {
        Deselect();
    }

    #endregion

    public void Select()
    {
        //visuals
        var mark = Marker.GetComponent<SpriteRenderer>();
        mark.enabled = true;
        
    }

    public void Deselect()
    {
        var mark = Marker.GetComponent<SpriteRenderer>();
        mark.enabled = false;
    }

}
