using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;

namespace ColonySim.Systems.Actions
{
    public interface IAction
    {
        bool Execute();
    }

    public interface IReversibleAction : IAction
    {
        bool UnExecute();
    }

    public class ActionHandler : System, ILogger
    {
        #region Static
        private static ActionHandler instance;
        public static ActionHandler Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=purple>[ACTION]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        private void Awake()
        {
            instance = this;
        }

        private readonly Stack<IReversibleAction> _UndoActions = new Stack<IReversibleAction>();
        private readonly Stack<IReversibleAction> _RedoActions = new Stack<IReversibleAction>();

        public static void Redo(int levels) { instance.Instance_Redo(levels); }

        private void Instance_Redo(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                if (_RedoActions.Count != 0)
                {
                    IReversibleAction cmd = _RedoActions.Pop();
                    if (!cmd.Execute())
                    {
                        Debug.LogError("Failed Redo Action!");
                    }
                    _UndoActions.Push(cmd);
                }
            }
        }

        public static void Undo(int levels) { instance.Instance_Undo(levels); }

        private void Instance_Undo(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                if (_UndoActions.Count != 0)
                {
                    IReversibleAction cmd = _UndoActions.Pop();
                    if (!cmd.UnExecute())
                    {
                        Debug.LogError("Failed Undo Action!");
                    }
                    _RedoActions.Push(cmd);
                }
            }
        }

        public static void InsertReversibleAction(IReversibleAction cmd)
        {
            instance.Instance_InsertReversibleAction(cmd);
        }

        private void Instance_InsertReversibleAction(IReversibleAction cmd)
        {
            _UndoActions.Push(cmd);
            _RedoActions.Clear();
        }
    }
}
