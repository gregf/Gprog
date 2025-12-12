using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositon = FindWallDirections(floorPositions, Direction2D.cardinalDirectionsList);
        foreach (var position in basicWallPositon)
        {
            tilemapVisualizer.paintSingleBasicWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;

                //if its false I know its a wall cause it never moved on that direction
                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    //get all the positins in a hashset
                    wallPositions.Add(neighbourPosition);
                }
            }
        }

        return wallPositions;
    }
}
