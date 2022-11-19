
using System;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Assets.Scripts.BuildingSystem;
using System.Collections;
using System.Linq;
using UnityEngine.Tilemaps;
using Assets.Scripts.AgentSystem;

namespace Assets.Scripts.Pathfinding
{
    [BurstCompile]
    public struct PathfindingJob : IJob
    {
        private const int DIAGONAL_COST = 14;
        private const int STRAIGHT_COST = 10;

        public int2 start;
        public int2 end;
        public int2 areaSize;
        public bool closestAvaliable;

        [ReadOnly]
        public NativeArray<bool> walkableArray;

        public NativeList<int2> path;

        public struct Node
        {
            public int x;
            public int y;

            public int index;

            public int GCost;
            public int HCost;
            public int FCost;

            public int CameFromIndex;

            public bool IsWalkable;

            //public byte Connections;

            public void UpdateFCost()
            {
                FCost = GCost + HCost;
            }
        }

        public void Execute()
        {
            AStar(start, end, areaSize, walkableArray, path, closestAvaliable);
        }

        private static void AStar(int2 start, int2 end, int2 areaSize, NativeArray<bool> walkableArray, NativeList<int2> path, bool comeNextTo = false)
        {
            NativeArray<Node> nodeArray = CreateGraph(end, areaSize, walkableArray, comeNextTo);
            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(4, Allocator.Temp);

            neighbourOffsetArray[0] = new int2(-1, 0); // Left
            neighbourOffsetArray[1] = new int2(+1, 0); // Right
            neighbourOffsetArray[2] = new int2(0, +1); // Up
            neighbourOffsetArray[3] = new int2(0, -1); // Down
            //neighbourOffsetArray[0] = new int2(-1, -1); // Left Down
            //neighbourOffsetArray[0] = new int2(-1, +1); // Left Up
            //neighbourOffsetArray[0] = new int2(+1, -1); // Right Down
            //neighbourOffsetArray[0] = new int2(+1, +1); // Right Up


            int endIndex = CalculateIndex(end.x, end.y, areaSize.x);

            if (walkableArray[endIndex] == false && comeNextTo == false)
            {
                goto path_reconstruction; //Sorry
            }
            else
            {


                Node startNode = nodeArray[CalculateIndex(start.x, start.y, areaSize.x)];
                startNode.GCost = 0;
                startNode.UpdateFCost();
                nodeArray[startNode.index] = startNode;

                NativeList<int> openList = new NativeList<int>(Allocator.Temp);
                NativeList<int> closedList = new NativeList<int>(Allocator.Temp);
                

                openList.Add(startNode.index);

                int currentNodeIndex;
                while (openList.Length > 0)
                {
                    currentNodeIndex = GetLowestFCostNodeIndex(openList, nodeArray);

                    Node currentNode = nodeArray[currentNodeIndex];

                    if (currentNodeIndex == endIndex)
                    {
                        break;
                    }

                    for (int i = 0; i < openList.Length; i++)
                    {
                        if (openList[i] == currentNodeIndex)
                        {
                            openList.RemoveAtSwapBack(i);
                            break;
                        }
                    }

                    closedList.Add(currentNodeIndex);

                    for (int i = 0; i < neighbourOffsetArray.Length; i++)
                    {
                        int2 neighbourOffset = neighbourOffsetArray[i];
                        int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                        if (!ValidatePosition(neighbourPosition, areaSize))
                        {
                            continue;
                        }

                        int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, areaSize.x);

                        if (closedList.Contains(neighbourNodeIndex))
                        {
                            continue;
                        }

                        Node neighbourNode = nodeArray[neighbourNodeIndex];
                        if (!neighbourNode.IsWalkable)
                        {
                            continue;
                        }

                        int2 currentNodePosition = new int2(currentNode.x, currentNode.y);

                        int tentativeGCost = currentNode.GCost + CalculateHeuristicCost(currentNodePosition, neighbourPosition);
                        if (tentativeGCost < neighbourNode.GCost)
                        {
                            neighbourNode.CameFromIndex = currentNodeIndex;
                            neighbourNode.GCost = tentativeGCost;
                            neighbourNode.UpdateFCost();
                            nodeArray[neighbourNodeIndex] = neighbourNode;

                            if (!openList.Contains(neighbourNode.index))
                            {
                                openList.Add(neighbourNode.index);
                            }
                        }
                    }
                }
                openList.Dispose();
                closedList.Dispose();
            }

            path_reconstruction:
            Node endNode = nodeArray[endIndex];
            if (endNode.CameFromIndex == -1)
            {
                //no path in range
                Debug.Log("Didn't find a path!");
                CalculatePath(start, nodeArray, endNode, path);
            }
            else
            {
                //found a path
                CalculatePath(start, nodeArray, endNode, path);
            }

            
            neighbourOffsetArray.Dispose();
            nodeArray.Dispose();
        }

        private static void CalculatePath(int2 startPos, NativeArray<Node> nodeArray, Node endNode, NativeList<int2> path)
        {
            if (endNode.CameFromIndex == -1)
            {
                path.Add(startPos);
            }
            else
            {
                path.Add(new int2(endNode.x, endNode.y));

                Node currentNode = endNode;
                while (currentNode.CameFromIndex != -1)
                {
                    Node cameFromNode = nodeArray[currentNode.CameFromIndex];
                    path.Add(new int2(cameFromNode.x, cameFromNode.y));
                    currentNode = cameFromNode;
                }
            }
        }

        private static bool ValidatePosition(int2 position, int2 areaSize)
        {
            return position.x < areaSize.x && position.y < areaSize.y && position.x > 0 && position.y > 0;
        }

        private static int GetLowestFCostNodeIndex(NativeList<int> openList, NativeArray<Node> nodeArray)
        {
            Node lowestCostNode = nodeArray[openList[0]];
            for (int i = 0; i < openList.Length; i++)
            {
                Node nodeToCheck = nodeArray[openList[i]];

                if (nodeToCheck.FCost < lowestCostNode.FCost)
                {
                    lowestCostNode = nodeToCheck;
                }
            }
            return lowestCostNode.index;
        }

        public static int CalculateIndex(int x, int y, int gridWidth)
        {
            return x + y * gridWidth;
        }

        private static int CalculateHeuristicCost(int2 start, int2 end)
        {
            //travel diagonally most of the time
            int x = math.abs(start.x - end.x);
            int y = math.abs(start.y - end.y);

            //the rest in a straight line
            int remaining = math.abs(x - y);

            return DIAGONAL_COST * math.min(x, y) + STRAIGHT_COST * remaining;
        }

        private static NativeArray<Node> CreateGraph(int2 end, int2 areaSize, NativeArray<bool> walkableArray, bool comeNextTo)
        {

            NativeArray<Node> nodeArray = new NativeArray<Node>(areaSize.x * areaSize.y, Allocator.Temp);
            //init
            for (int x = 0; x < areaSize.x; x++)
            {
                for (int y = 0; y < areaSize.y; y++)
                {
                    Node node = new Node();
                    node.x = x;
                    node.y = y;

                    node.index = CalculateIndex(x, y, areaSize.x);

                    node.GCost = int.MaxValue;
                    node.HCost = CalculateHeuristicCost(new int2(x, y), end);
                    node.UpdateFCost();

                    node.CameFromIndex = -1;//invalid value - meaning an encounter of the last node in the path

                    if (node.x == end.x && node.y == end.y && comeNextTo)
                    {
                        node.IsWalkable = true;
                    }
                    else
                    {
                        node.IsWalkable = walkableArray[node.index];
                    }

                    nodeArray[node.index] = node;
                }
            }
            return nodeArray;
        }

    }

    public class PathfindingManager : MonoBehaviour
    {
        public static readonly int MAP_X_SIZE = 256;
        public static readonly int MAP_Y_SIZE = 256;

        private NativeArray<bool> _walkableArray;

        #region Public Accesors

        public static NativeArray<bool> WalkableArray { get => Instance._walkableArray; }
        public static Vector3Int ZeroPointOffset { get; private set; }

        #endregion

        #region Events and Handlers

        public static event Action OnWalkableArrayChanged;
        
        #endregion

        #region UnityMethods

        public static PathfindingManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _walkableArray = new NativeArray<bool>(MAP_X_SIZE * MAP_Y_SIZE, Allocator.Persistent);
            ZeroPointOffset = new Vector3Int(-MAP_X_SIZE / 2, -MAP_Y_SIZE / 2);

            GameManager.OnMapChanged += UpdateWalkableArray;

            
            //TODO: This is temporary solution to make sure that update happens after the map has been loaded
            StartCoroutine(UpdateWalkableArrayNextFrame());
        }

        private void OnDestroy()
        {
            WalkableArray.Dispose();
        }

        #endregion

        #region PublicMethods

        public static Vector3Int ConvertToTilemapCoordinates(int2 vec)
        {
            return new Vector3Int(vec.x + ZeroPointOffset.x, vec.y + ZeroPointOffset.y);
        }

        public static Vector3Int ConvertToArrayCoordinates(Vector3Int vec)
        {
            return new Vector3Int(vec.x - ZeroPointOffset.x, vec.y - ZeroPointOffset.y);
        }

        public static PathfindingJob CreatePathfindingJob(Vector3Int startPos, Vector3Int endPos, bool closestAvaliable, NativeList<int2> path)
        {

            int2 areaSize = new int2(MAP_X_SIZE, MAP_Y_SIZE);
            //Vector3Int startEnd = endPos - startPos;

            Vector3Int convertedCoordinates = ConvertToArrayCoordinates(startPos);
            int2 start = new int2(convertedCoordinates.x, convertedCoordinates.y);

            convertedCoordinates = ConvertToArrayCoordinates(endPos);
            int2 end = new int2(convertedCoordinates.x, convertedCoordinates.y);


            path.Clear();

            return new PathfindingJob
            {
                start = start,
                end = end,
                areaSize = areaSize,
                closestAvaliable = closestAvaliable,
                walkableArray = WalkableArray,
                path = path,
            };
        }

        #endregion

        #region PrivateMethods


        /// <summary>
        /// Coroutine used to update a native array of walkable tiles one frame after its called
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateWalkableArrayNextFrame()
        {
            yield return null;
            UpdateWalkableArray();
        }

        private void UpdateWalkableArray()
        {

            for (int x = 0; x < MAP_X_SIZE; x++)
            {
                for (int y = 0; y < MAP_Y_SIZE; y++)
                {

                    bool value = false;
                    Vector3Int vec = ConvertToTilemapCoordinates(new int2(x, y));

                    if (GameManager.Instance.TilemapGround.GetTile(vec + new Vector3Int(0, 0, -1)) != null)
                    {
                        if (((IMapElement)GameManager.Instance.TilemapGround.GetTile(vec + new Vector3Int(0, 0, -1))).Walkable)
                        {
                            
                            if (GameManager.Instance.TilemapGround.GetTile(vec) == null)
                            {
                                value = true;
                            }
                            else if (((IMapElement)GameManager.Instance.TilemapGround.GetTile(vec)).CanWalkThrough)
                            {
                                value = true;
                            }
                            if(GameManager.Instance.TilemapColliders.GetTile(vec) != null)
                            {
                                if (((IMapElement)GameManager.Instance.TilemapColliders.GetTile(vec)).CanWalkThrough)
                                {
                                    value = true;
                                }
                                else
                                {
                                    value = false;
                                }
                            }
                            
                        }
                    }
                    


                    _walkableArray[x + y * MAP_X_SIZE] = value;

                }
            }
            OnWalkableArrayChanged?.Invoke();
        }


        #endregion


    }
}