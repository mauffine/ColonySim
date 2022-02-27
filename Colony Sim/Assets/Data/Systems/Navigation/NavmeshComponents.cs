using System.Collections;
using System.Collections.Generic;
using ColonySim.World.Tiles;
using ColonySim.World;
namespace ColonySim.Systems.Navigation
{
    public class Node
    {
        public Edge[] edges;
    }
    public class Edge
    {
        public int cost;
        public Node node;
    }
    public class NavMesh
    {
        Dictionary<WorldPoint,Node> nodes;

        //generate nodes for all tiles that don't have impassable terrain on them
        public bool GenerateNavMesh(World.GameWorld world)
        {
            //generate nodes for each walkable tile
            // TODO: this should pull the walkability and cost from the tile
            for (int x = 0; x < world.Size.x; x++)
                for (int y = 0; y < world.Size.y; y++)
                {
                    ITileData t = world.GetTileData(new WorldPoint(x, y));

                    if (t.Container.GetEntity("Concrete Wall") != null)
                    {
                        Node n = new Node();
                        nodes.Add(new WorldPoint(x,y),n);
                    }
                }

            //Traverse Nodes in the NavMesh and generate Edges to all the neighbouring Nodes
            // Cost for Edges is calculated as the cost to LEAVE the node and not cost to enter
            foreach  (WorldPoint p in nodes.Keys)
            {
                var neighbours = TileManager.Get.GetAdjacentTiles(p).AdjacentTiles;
                for (int i = 0; i < neighbours.Length; i++)
                {
                    if (neighbours[i] != null && nodes.ContainsKey(neighbours[i].Coordinates))
                    {
                        Edge e = new Edge();
                        // TODO: make this pull cost from tiledata
                        e.cost = 1;
                        e.node = nodes[neighbours[i]];
                    }
                }
            }
            return false;
        }
    }
}
