
using BuildingSystem;
using Utills;
using System;
using System.Collections.Generic;
using System.Linq;
using ItemSystem;

namespace GameStates
{
    /// <summary>
    /// This class is basically a manager of managers
    /// <br/>
    /// It enables and disables functionalities depending on the state
    /// <br/>
    /// State can be changed from the outside
    /// <br/>
    /// </summary>
    public partial class GameManager : SingletoneBase<GameManager>, IFiniteStateMachine<GameSateBase>
    {
        public List<Storage> StorageBuildings;

        public Dictionary<Type, IScriptEnabler> scriptEnablers;

        public GameSateBase CurrentState { get; private set; }

        #region UnityMethods

        void Start()
        {
            SwitchState(new DefaultState());
        }

        private void OnEnable()
        {
            StorageBuildings = FindObjectsOfType<Storage>().ToList();//TODO: repeat when map changed

            MapManager.OnMapChanged += (e) => StorageBuildings = FindObjectsOfType<Storage>().ToList();

            scriptEnablers = new Dictionary<Type, IScriptEnabler>();

            scriptEnablers = 
                GetComponentsInChildren<IScriptEnabler>().ToDictionary((e) => e.GetType());
        }

        #endregion

        public void SwitchState(GameSateBase state)
        {
            CurrentState = state;
            CurrentState.EnterState(this);
        }



    }
}