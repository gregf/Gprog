using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomDungeonGenerator : RandomWalkDungeonGenerator
{
    [SerializeField]private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField][Range(0,10)]private int offset = 1;
    [SerializeField]private bool randomWalkRooms = false;

    public UnityEvent OnFinishedRoomGeneration;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning
            (new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)),minRoomWidth, minRoomHeight);


        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateRooms(roomsList);
        }
        
        List<Vector2Int> roomCenter = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenter.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenter);
        floor.UnionWith(corridors);

        tilemapVisualizer.paintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);

            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) 
                    && position.x <= (roomBounds.xMax - offset) 
                    && position.y >= (roomBounds.yMin - offset) 
                    && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        OnFinishedRoomGeneration?.Invoke();
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenter)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenter[Random.Range(0, roomCenter.Count)];
        roomCenter.Remove(currentRoomCenter);

        while (roomCenter.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenter);
            roomCenter.Remove(closest);

            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);

        //goes up or down until it reaches the destination
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        //goes left or right until it reaches the destination
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }

        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenter)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance =float.MaxValue;

        //iteretes through all room centers and if it finds one thats close the the one
        //that it is iterating now it becomes the closest
        foreach (var position in roomCenter)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int collum = offset; collum < room.size.x - offset; collum++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(collum, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
