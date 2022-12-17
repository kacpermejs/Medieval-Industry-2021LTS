namespace AgentSystem
{



    public partial class Walker
    {
        public abstract class WalkerStateBase : IState<Walker>
        {
            public abstract void EnterState(Walker obj);

            public abstract void UpdateState(Walker obj);

            public abstract void ExitState(Walker obj);
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