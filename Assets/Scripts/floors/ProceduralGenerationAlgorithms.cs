using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*   **********************************************************************
since im only gonna need different paramiters to create the dungeon,
the classes im making are static classes that dont have instatnce variable
thus calling them by the name of the class to make my script more readable
     **********************************************************************
*/

public static class ProceduralGenerationAlgorithms
{

    //hashSet overrides some methods and allows us to use some boolean based methods like unionWith
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int StartPosition, int walkLength)
    {
        //makes a path that will be returned at the end as the dungeon floor
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(StartPosition);
        var previousPosition = StartPosition;

        //loop through the path length calling the random direction to make the floor and adds it to the path
        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }
        return path;
    }

    //using list instead of hashset to be able to access the last position 
    //later i'll use hashset to store the positions in order to not have duplicates
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPosition = startPosition;
        corridor.Add(currentPosition);

        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }

        return corridor;
    }


    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minimumWidth, int minimumHeight)
    {
        //first in first out stracture for the queue
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();

        //automaticaly added and removed with enqueue
        roomsQueue.Enqueue(spaceToSplit);
        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();
            if (room.size.y >= minimumHeight && room.size.x >= minimumWidth)
            {
                if (Random.value < 0.5f)//contidion could be removed but it give a less acurate split
                {
                    if (room.size.y >= minimumHeight * 2)
                    {
                        SplitHorizontaly(minimumWidth, roomsQueue, room);
                    }
                    else if (room.size.x >= minimumWidth * 2)
                    {
                        SplitVerticaly(minimumHeight, roomsQueue, room);
                    }
                    else if(room.size.x >= minimumWidth && room.size.y >= minimumHeight)
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minimumWidth * 2)
                    {
                        SplitVerticaly(minimumHeight, roomsQueue, room);
                    }
                    else if (room.size.y >= minimumHeight * 2)
                    {
                        SplitHorizontaly(minimumWidth, roomsQueue, room);
                    }
                    else if(room.size.x >= minimumWidth && room.size.y >= minimumHeight)
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }
        return roomsList;
    }

    private static void SplitVerticaly(int minimumHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        //The range doesn't include the max value so I never add -1 to the rooms size
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontaly(int minimumWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        //if i wanted more equal rooms then Random.range(minHeight, room.size.y - minHeight)
        var ySplit = Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));

        roomsQueue.Enqueue(room1); 
        roomsQueue.Enqueue(room2);
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0 , 1), //UP
        new Vector2Int(1 , 0), //RIGHT
        new Vector2Int(0 , -1), //DOWN
        new Vector2Int(-1 , 0) //LEFT 
        
    };

    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1 , 1), //UP-RIGHT
        new Vector2Int(1 , -1), //RIGHT-DOWN
        new Vector2Int(-1 , -1), //DOWN-LEFT
        new Vector2Int(-1 , 1) //LEFT-UP

    };

    public static List<Vector2Int> allDirectionList = new List<Vector2Int>
    {
        new Vector2Int(0 , 1), //UP
        new Vector2Int(1 , 1), //UP-RIGHT
        new Vector2Int(1 , 0), //RIGHT
        new Vector2Int(1 , -1), //RIGHT-DOWN
        new Vector2Int(0 , -1), //DOWN
        new Vector2Int(-1 , -1), //DOWN-LEFT
        new Vector2Int(-1 , 0), //LEFT 
        new Vector2Int(-1 , 1) //LEFT-UP

    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        /*
        gets a ranom direction from 0 to this case 3 (from the list created above)
        dynamicaly so in the future diagonal directions could be added
        */
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}