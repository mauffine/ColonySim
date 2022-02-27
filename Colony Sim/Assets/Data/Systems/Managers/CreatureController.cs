using UnityEngine;
using System.Collections.Generic;

using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;
using ColonySim.Creatures;
using ColonySim.World;

namespace ColonySim.Systems
{
    public class CreatureController : System, ILogger, IWorldTick
    {
        #region Static
        private static CreatureController instance;
        public static CreatureController Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=red>[CREATURES]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        [SerializeField]
        private bool drawCharactersGizmo;

        private List<ICreature> simulatedCreatures;

        public static TestCreature TEST_CREATURE => (TestCreature)instance.simulatedCreatures[0];

        public override void Init()
        {
            this.Notice("<color=blue>[Creature Controller Init]</color>");
            instance = this;
            base.Init();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            WorldSimulation.Get.Simulate(this);

            CreateCreature(new WorldPoint(3, 3));
        }

        public void CreateCreature(WorldPoint Coordinate)
        {
            TestCreature testCreature = new TestCreature();
            testCreature.SetTilePosition(Coordinate);

            if (simulatedCreatures == null) simulatedCreatures = new List<ICreature>() { testCreature };
            else simulatedCreatures.Add(testCreature);
        }

        public void WorldTick(float delta)
        {
            foreach (var creature in simulatedCreatures)
            {
                creature.WorldTick(delta);
            }
        }

        public void OnDrawGizmos()
        {
            if (Initialized)
            {
                if (simulatedCreatures != null)
                {
                    if (drawCharactersGizmo)
                    {
                        foreach (var creature in simulatedCreatures)
                        {
                            Gizmos.color = new Color(1f, 0.8f, 0.7f, 1f);
                            Vector3 nosePosition = new Vector3(creature.Navigation.Position.x, creature.Navigation.Position.y + 0.2f);
                            Gizmos.DrawCube(creature.Navigation.Position, new Vector3(0.8f, 0.2f, 0.2f));
                            Gizmos.DrawCube(nosePosition, new Vector3(0.1f, 0.2f, 0.1f));

                            Gizmos.color = new Color(0.6f, 1, 0.25f, 1f);
                            Gizmos.DrawCube(creature.Navigation.Position, new Vector3(0.5f, 0.35f, 0.5f));
                            
                            
                            
                        }
                    }
                }
            }
        }
    }
}