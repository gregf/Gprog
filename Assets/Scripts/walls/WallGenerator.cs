using System;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositon = FindWallDirections(floorPositions, Direction2D.cardinalDirectionsList);
        var cornerWallPositions = FindWallDirections(floorPositions, Direction2D.diagonalDirectionsList);

        CreateBasicWalls(tilemapVisualizer, basicWallPositon, floorPositions);
        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);
    }

    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.allDirectionList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }

            tilemapVisualizer.paintSingleCornerWall(position, neighboursBinaryType);
        }
    }

    private static void CreateBasicWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositon, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in basicWallPositon)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.paintSingleBasicWall(position, neighboursBinaryType);
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
