using System.Collections;
using UnityEngine;
using System;

namespace AgentSystem
{
    public partial class Walker
    {
        public class HoldState : WalkerStateBase
        {
            private int seconds;
            private Action callback;

            private WalkingState savedState;

            public HoldState(int seconds, Action callback)
            {
                this.seconds = seconds;
                this.callback = callback;
            }

            public override void EnterState(Walker obj)
            {
                if (obj.CurrentState is WalkingState state)
                {
                    savedState = state;
                }
                obj.StartCoroutine(HoldCoroutine(obj, seconds));
            }

            public override void ExitState(Walker obj)
            {
                callback();
            }

            public override void UpdateState(Walker obj) { }

            private IEnumerator HoldCoroutine(Walker obj, int seconds)
            {

                yield return new WaitForSeconds(seconds);

                if (savedState != null)
                {
                    obj.SwitchState(savedState);
                }
                else
                {
                    obj.SwitchState(new IdleState());
                }
            }
        }
    }
}


/*if (!_busy && _command != null)
            {
                _command = null;
            }*/

/*if (_mouseMovement)
{
    if (Input.GetMouseButtonDown(0))
    {
        //Do not place any object if mouse is over a UI object
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int endPoint = GameManager.ConvertToGridPosition(screenPoint);

            var manualMoveCommand = new MoveCommand(this, endPoint);

            AddCommand(manualMoveCommand);
            StartCoroutine(ExecuteCommand(true));

        }
    }
}*/