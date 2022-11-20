using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.AgentSystem
{
    public partial class AgentSelector
    {
        public static void Cancel()
        {
            
        }

        public static void ConfirmSelectionCallBack()
        {

        }

        internal static void NotifyAgentSelected(SelectableAgent agent)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                AgentSelector.ReplaceAll(agent);
            }
            else
            {
                AgentSelector.AddAnotherAgent(agent);
            }
        }

        public class State
        {
            public abstract class Base : IState<AgentSelector>
            {
                public abstract void EnterState(AgentSelector obj);

                public abstract void UpdateState(AgentSelector obj);
            }

            public class WorkerSelection : Base
            {
                public override void EnterState(AgentSelector obj)
                {
                
                }

                public override void UpdateState(AgentSelector obj)
                {
                
                }
            }

            public class MovementSelection : Base
            {

                public override void EnterState(AgentSelector obj)
                {
                    
                }

                public override void UpdateState(AgentSelector obj)
                {
                    
                }
            }
        }
    }
}