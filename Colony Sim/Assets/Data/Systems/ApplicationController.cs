using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColonySim.Systems
{
    /// <summary>
    /// Main Thread
    /// </summary>
    public class ApplicationController : MonoBehaviour
    {
        #region Static
        private static ApplicationController instance;
        public static ApplicationController Get() => instance;

        #endregion

        [SerializeField]
        private System[] Systems;

        private System[] InitializedSystems;
        private int InitializedCount;
        private bool Initialised;
        
        // Start is called before the first frame update
        void Awake()
        {
            instance = this;
            if (Systems != null)
            {
                InitializedSystems = new System[Systems.Length];
                foreach (var sys in Systems)
                {
                    sys.Init();
                }
            }           
        }

        // Update is called once per frame
        void Update()
        {
            if (Initialised)
            {
                foreach (var sys in Systems)
                {
                    sys.Tick();
                }
            }       
        }

        public void Initialized(System sys)
        {
            Debug.Log("Init!!");
            InitializedSystems[InitializedCount] = sys;
            InitializedCount++;
            if (InitializedCount >= InitializedSystems.Length)
            {
                FinishInit();
            }
        }

        public void FinishInit()
        {
            Initialised = true;
            foreach (var sys in InitializedSystems)
            {
                sys.OnInitialized();
            }
        }
    }
}
