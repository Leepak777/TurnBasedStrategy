using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

public class Pathfinder
{
    private TileManager tileM;
    private Teleport teleport;

    public Pathfinder(TileManager tileM, Teleport teleport)
    {
        this.tileM = tileM;
        this.teleport = teleport;
    }

    public bool GenerateAStarPath(Vector3Int start, Vector3Int end, out List<Vector3Int> path, float tilescheck)
    {
        path = new List<Vector3Int>();

        // Set up data structures
        HashSet<Vector3Int> openSet = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, float> gScores = new Dictionary<Vector3Int, float>();
        Dictionary<Vector3Int, float> fScores = new Dictionary<Vector3Int, float>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        openSet.Add(start);
        gScores[start] = 0;
        fScores[start] = tileM.GetDistance(start, end);

        // Main loop
        while (openSet.Count > 0)
        {
            // Get node with lowest fScore
            Vector3Int current = openSet.OrderBy(pos => fScores.ContainsKey(pos) ? fScores[pos] : float.MaxValue).First();

            // If end node is found, reconstruct path and return true
            if (current == end)
            {
                ReconstructPath(cameFrom, start, end, path);
                return true;
            }

            openSet.Remove(current);

            // Loop through neighbours and update scores
            foreach (KeyValuePair<Vector3Int, float> neighbour in GetNeighbourNodes(current, tilescheck))
            {
                float tentativeGScore = gScores[current] + neighbour.Value;

                if (!gScores.ContainsKey(neighbour.Key) || tentativeGScore < gScores[neighbour.Key])
                {
                    cameFrom[neighbour.Key] = current;
                    gScores[neighbour.Key] = tentativeGScore;
                    fScores[neighbour.Key] = gScores[neighbour.Key] + tileM.GetDistance(neighbour.Key, end);

                    if (!openSet.Contains(neighbour.Key))
                    {
                        openSet.Add(neighbour.Key);
                    }
                }
            }
        }

        // Path not found
        return false;
    }

    private void ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int start, Vector3Int end, List<Vector3Int> path)
    {
        // Reconstruct path
        Vector3Int pathPos = end;
        while (cameFrom.ContainsKey(pathPos))
        {
            path.Insert(0, pathPos);
            pathPos = cameFrom[pathPos];
        }
        path.Insert(0, start);
    }

    public Dictionary<Vector3Int, float> GetNeighbourNodes(Vector3Int pos, float tilescheck)
    {
        Dictionary<Vector3Int, float> neighbours = new Dictionary<Vector3Int, float>();

        int x = pos.x;
        int y = pos.y;
        bool evenColumn = (y % 2 == 0);

        // Check adjacent tiles
        if (evenColumn)
        {
            int[,] adjacentOffsets = new int[,] {
                {1, 0},
                {0, 1},
                {-1, 1},
                {-1, 0},
                {-1, -1},
                {0, -1},
            };

            for (int i = 0; i < adjacentOffsets.GetLength(0); i++)
            {
                int dx = adjacentOffsets[i, 0];
                int dy = adjacentOffsets[i, 1];

                Vector3Int neighbourPos = new Vector3Int(x + dx, y + dy, pos.z);

                // Check if the node is walkable and in range
                if (tileM.inArea(pos, neighbourPos, tilescheck) && tileM.GetNodeFromWorld(neighbourPos).walkable)
                {
                    float distance = tileM.GetDistance(pos, neighbourPos);
                    neighbours.Add(neighbourPos, distance);
                }
            }
        }
        else
        {
            int[,] adjacentOffsets = new int[,] {
                {1, 0},
                {1, 1},
                {0, 1},
                {-1, 0},
                {0, -1},
                {1, -1},
            };

            for (int i = 0; i < adjacentOffsets.GetLength(0); i++)
            {
                int dx = adjacentOffsets[i, 0];
                int dy = adjacentOffsets[i, 1];

                Vector3Int neighbourPos = new Vector3Int(x + dx, y + dy, pos.z);

                // Check if the node is walkable and in range
                if (tileM.inArea(pos, neighbourPos, tilescheck) && tileM.GetNodeFromWorld(neighbourPos).walkable)
                {
                    float distance = tileM.GetDistance(pos, neighbourPos);
                    neighbours.Add(neighbourPos, distance);
                }
            }
        }

        return neighbours;
    }


}
