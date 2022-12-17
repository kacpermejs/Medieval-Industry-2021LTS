using System;
using UnityEngine;
using System.Collections.Generic;
using AgentSystem;
using Utills;
using AgentSystem.Movement;
using UnityEngine.EventSystems;
using GameStates;

namespace AgentSystem
{
    public partial class AgentSelectionManager : SingletoneBase<AgentSelectionManager>, IScriptEnabler
    {
        [SerializeField] private Transform _selectionTransform;
        
        private List<ISelectableAgent> _agentList;


        private Vector2 _startPosition;
        private Camera _camera;

        public IReadOnlyCollection<ISelectableAgent> AgentList => _agentList.AsReadOnly();
        public bool AlwaysActive => false;

        public event Action OnSelectionChanged;

        #region Unity methods

        private void Awake()
        {
            _camera = Camera.main;
            _agentList = new List<ISelectableAgent>();
        }

        private void OnEnable()
        {
            
        }

        private void Update()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //Clear on right mouse button
                if (Input.GetMouseButtonDown(1))
                {
                    ClearAgentList();
                }

                if (Input.GetMouseButton(0))//visuals
                {
                    ResizeSelectionArea();
                }

                //Start selection area
                if (Input.GetMouseButtonDown(0))
                {
                    _selectionTransform.gameObject.SetActive(true);//visuals
                    _selectionTransform.localScale = Vector3.zero;

                    _startPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                    //keep selected when left ctrl pressed
                        
                }

                if (Input.GetMouseButtonUp(0))
                {
                    _selectionTransform.gameObject.SetActive(false);//visuals

                    Vector2 endPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                    Collider2D[] colliders = Physics2D.OverlapAreaAll(_startPosition, endPosition);

                    //keep selected units if left ctrl is pressed
                    bool doClear = !Input.GetKey(KeyCode.LeftControl);

                    foreach (var unit in colliders)
                    {
                        if (unit.TryGetComponent<ISelectableAgent>(out var selectedUnit))
                        {
                            if(doClear)
                            {
                                ClearAgentList();
                                doClear = false;
                            }
                            selectedUnit.Select();
                            _agentList.Add(selectedUnit);
                            OnSelectionChanged?.Invoke();//this will not fire when there was 0 units selected
                        }
                    }
                }
            }
        }

        #endregion

        public void Enable()
        {
            this.enabled = true;
        }

        public void Disable()
        {
            this.enabled = false;
        }

        public static void ClearAgentList()
        {
            foreach (var agent in Instance._agentList)
            {
                agent.Deselect();
            }
            Instance._agentList.Clear();
        }

        private void ResizeSelectionArea()
        {
            Vector2 currentPos = _camera.ScreenToWorldPoint(Input.mousePosition);

            Vector2 lowerLeft = new Vector2(
                Mathf.Min(_startPosition.x, currentPos.x),
                Mathf.Min(_startPosition.y, currentPos.y)
                );
            Vector2 upperRight = new Vector2(
                Mathf.Max(_startPosition.x, currentPos.x),
                Mathf.Max(_startPosition.y, currentPos.y)
                );

            _selectionTransform.position = lowerLeft;
            _selectionTransform.localScale = upperRight - lowerLeft;
        }
    }
}

