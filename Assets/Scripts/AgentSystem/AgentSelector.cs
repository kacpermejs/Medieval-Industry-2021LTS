using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.AgentSystem;
using Assets.Scripts.Utills;
using Assets.Scripts.AgentSystem.Movement;
using UnityEngine.EventSystems;

namespace Assets.Scripts.AgentSystem
{
    public partial class AgentSelector : SingletoneBase<AgentSelector>
    {
        [SerializeField] private Transform _selectionTransform;
        
        private List<ISelect> _agentList;

        public IReadOnlyCollection<ISelect> AgentList => _agentList.AsReadOnly();

        private bool _doSelect = false;

        private Vector2 _startPosition;
        private Camera _camera;

        public event Action OnSelectionChanged;

        #region Unity methods

        private void Awake()
        {
            _camera = Camera.main;

            GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
            _agentList = new List<ISelect>();
        }

        private void Update()
        {
            if (_doSelect)
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
                            if (unit.TryGetComponent<ISelect>(out var selectedUnit))
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

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
        }

        #endregion

        public static void ClearAgentList()
        {
            foreach (var agent in Instance._agentList)
            {
                agent.Deselect();
            }
            Instance._agentList.Clear();
        }

        private void GameManagerOnGameStateChanged(GameState obj)
        {
            switch (obj)
            {
                case GameState.Default:
                    _doSelect = true;
                    break;
                case GameState.UnitCommanding:
                    _doSelect = true;
                    break;
                default:
                    _doSelect = false;
                    break;
            }
        }



    }
}

