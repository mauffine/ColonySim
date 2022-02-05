using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Systems
{
    public class ApplicationController : MonoBehaviour
    {
        public List<System> Systems;

        private List<System> InitializedSystems;

        // Start is called before the first frame update
        void Awake()
        {
            foreach (var sys in Systems)
            {
                if (sys.Init())
                {

                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var sys in Systems)
            {
                sys.Tick();
            }
        }
    }
}
