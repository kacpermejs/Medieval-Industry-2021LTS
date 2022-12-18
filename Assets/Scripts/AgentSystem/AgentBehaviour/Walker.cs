using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Utills;
using GameStates;
using System;

namespace AgentSystem
{
    public interface IActorMove
    {
        void Move(Vector3Int targetPosition, Action callback);
    }

    public interface IActorHold
    {
        void HoldForSeconds(int seconds, Action callback);

    }



    public partial class Walker : AgentBehaviour, IActorMove, IActorHold, IFiniteStateMachine<Walker.WalkerStateBase>
    {
        public Transform MovePoint;

        public const float Y_OFFSET = 0.22f;
        public const float Z_OFFSET = 1;
        public float MoveSpeed = 2f;
        //[SerializeField] private bool _mouseMovement = false;



        public WalkerStateBase CurrentState { get; private set; }
        //public float MoveSpeed { get => _moveSpeed; }

        #region UnityMethods


        private void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            MovePoint.parent = null;
            SwitchState(new IdleState());

        }

        // Update is called once per frame
        void Update()
        {
            CurrentState.UpdateState(this);


            /*
            if (!_busy && _command != null)
            {
                StartCoroutine(ExecuteCommand(false));
            }*/

        }
        private void OnDestroy()
        {

        }

        #endregion

        public void Move(Vector3Int targetPosition, Action callback)
        {
            //Agent will follow the path starting the next frame from execution (not including the waiting queue)
            SwitchState(new WalkingState(targetPosition, callback));

        }

        public void HoldForSeconds(int seconds, Action callback)
        {
            SwitchState(new HoldState(seconds, callback));
        }


        public void SwitchState(WalkerStateBase state)
        {
            CurrentState?.ExitState(this);
            CurrentState = state;
            CurrentState.EnterState(this);
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