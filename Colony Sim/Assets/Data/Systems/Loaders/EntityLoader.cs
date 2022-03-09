using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ColonySim.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using ILoggerSlave = ColonySim.LoggingUtility.ILoggerSlave;

namespace ColonySim.Entities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class EntityDef : Attribute { }
}

namespace ColonySim.Systems
{
    public class EntityLoader : ILoggerSlave
    {
        public ILogger Master => LoadingSystem.Get;
        public string LoggingPrefix => "<color=blue>[ENTITYLOADER]</color>";

        public Dictionary<string, IEntity> LoadComponentsFromAssembly()
        {
            this.Notice("Loading Entities From Assembly...");
            IEnumerable<Type> _defs = FindClassTypes<EntityDef>(Assembly.GetExecutingAssembly());
            Dictionary<string, IEntity> ret = new Dictionary<string, IEntity>();

            foreach (var def in _defs)
            {
                IEntity entity = (IEntity)Activator.CreateInstance(def, new object[] { });
                ret.Add(entity.DefName, entity);
                this.Verbose($"Loaded Entity::{entity.DefName}");
            }
            return ret;
        }

        private IEnumerable<Type> FindClassTypes<T>(Assembly assembly)
        {
            foreach (Type Def in assembly.GetTypes())
            {
                if (Def.GetCustomAttributes(typeof(T), true).Length > 0)
                {
                    yield return Def;
                }
            }
        }
    }
}
