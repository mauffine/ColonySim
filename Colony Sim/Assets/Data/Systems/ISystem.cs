using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Systems
{
    /// <summary>
    /// main.Thread() Process
    /// </summary>
    public interface ISystem
    {
        public void Init();
        public void OnInitialized();
        public void Tick();

        public bool Initialized { get; }
    }

    public abstract class System : MonoBehaviour, ISystem
    {
        public bool Initialized { get; protected set; }
        public virtual void Init() { Initialized = true; ApplicationController.Get().Initialized(this); }
        public virtual void OnInitialized() { }
        public virtual void Tick() { }
    }

}
