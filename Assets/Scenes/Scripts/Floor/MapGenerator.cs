using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private const float FAIL_CHANCE = 0.5f;

    static void ShuffleList(List<Vector2Int> l){
        for(int i = l.Count - 1; i >= 1; i--){
            int swap = Random.Range(0, i + 1);
            (l[i], l[swap]) = (l[swap], l[i]);
        }
    }

    //generate random map 
    public static Map RandomMap(int width, int height, int minRooms, int maxRooms, float failChance, float itemChance)
    {
        Map map = new(width, height);
       //get cell in map 

        List<Vector2Int> neighborOffsets = new()
        {
            new(0, 1),
            new(0, -1),
            new(1, 0),
            new(-1, 0),
        };

        //begin with random cell 
        Vector2Int startCell = new(
            Random.Range(0, width),
            Random.Range(0, height)
        );

        
        Stack<Vector2Int> stack = new();
        stack.Push(startCell);

        int numRooms = 0;
        // continue as long as there are rooms left 
        while (stack.Count > 0)
        {
            // get the unexplored room
            Vector2Int current = stack.Pop();

            
            if (!map.Get(current.x, current.y).IsEmpty)
            {
                continue;
            }

            // Save neighboring positions:
            List<Vector2Int> selectedNeighbors = new();

        
            ShuffleList(neighborOffsets);

            foreach (Vector2Int offset in neighborOffsets)
            {
                Vector2Int neighbor = current + offset;

                // if the neighbor is not in bounds or
                // is not empty, disregard it
                if (
                    !map.InBounds(neighbor.x, neighbor.y)
                    || !map.Get(neighbor.x, neighbor.y).IsEmpty
                )
                {
                    continue;
                }

                
                int neighboringRooms = 0;
                foreach (Vector2Int neighborOffset in neighborOffsets)
                {
                    Vector2Int neighborNeighbor = neighbor + neighborOffset;

                    if (
                        map.InBounds(neighborNeighbor.x, neighborNeighbor.y)
                        && !map.Get(neighborNeighbor.x, neighborNeighbor.y).IsEmpty
                    )
                    {
                        neighboringRooms++;
                    }
                }
                if (neighboringRooms > 1)
                {
                    continue;
                }
                selectedNeighbors.Add(neighbor);
            }

            // add neighbor to stsack
            int neighborsToAdd = Random.Range(1, selectedNeighbors.Count + 1);
            for(int i = 0; i < neighborsToAdd; i++)
            {
                stack.Push(selectedNeighbors[i]);
            }

            // the last room is boss room. 
            bool spawnBoss = false;
            if (numRooms >= maxRooms - 1)
            {
                spawnBoss = true;
            }

            // if we are at the minimum number of rooms,
            // stop generating rooms with a random chance
            if (numRooms >= minRooms - 1 && Random.value < failChance)
            {
                spawnBoss = true;
            }

            if (spawnBoss)
            {
                map.Get(current.x, current.y).Type = MapEntryType.BossRoom;
                // the boss is the last room, so stop iterating
                break;
            }

            MapEntryType roomType = MapEntryType.NormalRoom;
            if (current == startCell)
            {
                roomType = MapEntryType.StartRoom;
            }
            else if (Random.value < itemChance)
            {
                roomType = MapEntryType.ItemRoom;
            }
            map.Get(current.x, current.y).Type = roomType;
            numRooms++;
        }
        return map;
    }
}