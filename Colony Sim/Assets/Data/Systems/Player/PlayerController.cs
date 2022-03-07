using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using UnityEngine.InputSystem;
using static ColonySim.InputControlMap;
using ColonySim.World;
using ColonySim.Systems;
using ColonySim.Systems.Tasks;
using InputSystem = ColonySim.Systems.InputSystem;
using ColonySim.Entities;
using ColonySim.Creatures;

namespace ColonySim
{
    public class PlayerController : ColonySim.Systems.System, ILogger, IPlayerActions
    {
        #region Static
        private static PlayerController instance;
        public static PlayerController Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=purple>[PLAYER]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        private ICreature selectedCreature;

        public override void Init()
        {
            this.Notice("<color=blue>[Player Controller Init]</color>");
            instance = this;
            base.Init();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            InputSystem.PlayerActions.SetCallbacks(this);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ITileData Data = CursorSystem.CursorTile;
                if (Data != null)
                {
                    MoveToWaypointTask newTask = new MoveToWaypointTask(Data.Coordinates);
                    if (selectedCreature != null && selectedCreature is IWorker Worker)
                    {
                        this.Notice($"Creating Interrupt Move..");
                        Worker.AssignTask(newTask, TaskAssignmentMethod.CLEAR);
                    }
                    else
                    {                       
                        TaskSystem.Delegate(newTask);
                    }                   
                }
            }
        }

        public void OnSelect(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ITileData Data = CursorSystem.CursorTile;
                if (Data != null)
                {
                    if (Data.Creature != null)
                    {
                        this.Notice($"Selected Creature!");
                        selectedCreature = Data.Creature;
                    }
                    else
                    {
                        IEntity TopEntity = Data.Container.First;
                        if (TopEntity != null)
                        {
                            this.Notice($"Selected {TopEntity.DefName}");
                        }
                        selectedCreature = null;
                    }
                    
                }
            }
        }
    }
}
