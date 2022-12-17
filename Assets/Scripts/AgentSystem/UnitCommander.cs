using System;
using UnityEngine;
using GameStates;
using Utills;
using UnityEngine.EventSystems;
using BuildingSystem;
using TaskSystem;

namespace AgentSystem
{

    public class UnitCommander : SingletoneBase<UnitCommander>, IScriptEnabler
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Do not place any object if mouse is over a UI object
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Vector2 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    Vector3Int endPosition = GetEndPosition(screenPoint);

                    

                }
            }
        }

        private static Vector3Int GetEndPosition(Vector2 screenPoint)
        {
            return MapManager.ConvertToGridPosition(screenPoint);
        }

        public void Disable()
        {
            enabled = false;
        }

        public void Enable()
        {
            enabled = true;
        }

        public static void AddAllSellectedWorkersToWorkplace(Workplace workplace)
        {
            foreach (var agent in AgentSelectionManager.Instance.AgentList)
            {
                if (agent is Agent agent2)
                {
                    if (agent2.TryGetComponent<Worker>(out Worker worker))
                    {
                        workplace.AddWorker(worker);
                    }
                }
            }
        }
    }
}
