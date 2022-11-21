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
                        Clear();
                    }

                    //Start selection area
                    if (Input.GetMouseButtonDown(0))
                    {

                        if (!Input.GetKey(KeyCode.C))
                        {
                            Clear();
                        }

                        _startPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        Vector2 endPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                        Collider2D[] colliders = Physics2D.OverlapAreaAll(_startPosition, endPosition);

                        
                        foreach (var unit in colliders)
                        {
                            if (unit.TryGetComponent<ISelect>(out var selectedUnit))
                            {
                                selectedUnit.Select();
                                OnSelectionChanged?.Invoke();
                                _agentList.Add(selectedUnit);
                            }
                        }
                    }

                }
            }
         

        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
        }

        #endregion

        public static void Clear()
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
                case GameState.WorkerAssignment:
                    //selection needs confirmation
                    _doSelect = true;
                    break;
                default:
                    _doSelect = false;
                    break;
            }
        }

        /*public static void SwitchState(State.Base state)
        {
            Instance.CurrentSelectorState = state;
            Instance.CurrentSelectorState.EnterState(Instance);
            //Debug.Log("State changed!");
        }*/

        private static void Add(ISelect agent)
        {
            Instance._agentList.Add(agent);
        }

        private static void ClearSelection()
        {
            foreach (var member in Instance._agentList)
            {
                member.Deselect();
            }

            Instance._agentList.Clear();

        }


    }
}

