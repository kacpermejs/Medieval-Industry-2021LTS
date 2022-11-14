using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableAgent : MonoBehaviour
{
    Transform SelectionMarker;

    #region UnityMethods

    private void Awake()
    {
        SelectionMarker = transform.Find("SelectionMarker");
    }

    private void OnEnable()
    {
        Select();
    }

    private void OnDisable()
    {
        Deselect();
    }

    #endregion

    public void Select()
    {
        var mark = SelectionMarker.GetComponent<SpriteRenderer>();
        mark.enabled = true;
    }

    public void Deselect()
    {
        var mark = SelectionMarker.GetComponent<SpriteRenderer>();
        mark.enabled = false;
    }

}
