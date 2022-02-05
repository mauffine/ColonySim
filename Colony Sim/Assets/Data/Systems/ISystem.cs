using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim
{
    public interface ISystem
    {
        public bool Init();
        public void Tick();
    }

    public class System : ISystem
    {
        public virtual bool Init() { return true; }
        public virtual void Tick() { }
    }

}
