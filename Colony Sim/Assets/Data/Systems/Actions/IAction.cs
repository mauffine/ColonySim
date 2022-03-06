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

    public static class IAutoExecExtension
    {
        public static bool AutoExec(this IReversibleAction self)
        {
            if (self.Execute())
            {
                ActionHandler.InsertReversibleAction(self);
                return true;
            }
            return false;
        }
    }
}
