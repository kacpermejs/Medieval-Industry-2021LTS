using System;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using System.Linq;
using Unity.Burst;
using Unity.VisualScripting;

namespace Assets.Scripts.Pathfinding
{


    public enum Directions : byte
    {
        North = 0b10000000,
        NorthEast = 0b01000000,
        East = 0b00100000,
        SouthEast = 0b00010000,
        South = 0b00001000,
        SouthWest = 0b00000100,
        West = 0b00000010,
        NorthWest = 0b0000001,
    }

    [BurstCompile]
    public struct PathfindingJob : IJob
    {
        private const int DIAGONAL_COST = 20;
        private const int STRAIGHT_COST = 10;

        public int2 start;
        public int2 end;
        public int2 areaSize;
        public bool closestAvaliable;

        [ReadOnly]
        public NativeArray<bool> walkableArray;

        public NativeList<int2> path;

        public struct Node2 : IEquatable<Node2>
        {
            public int2 position;

            public int GCost;
            public int HCost;
            public int FCost;

            public bool WasVisited;

            public int2 CameFrom;

            public void UpdateFCost()
            {
                FCost = GCost + HCost;
            }

            public bool Equals(Node2 other)
            {
                return this.position.x == other.position.x && this.position.y == other.position.y;
            }
        }

        public struct Node
        {
            public int2 position;

            public int index;

            public int GCost;
            public int HCost;
            public int FCost;

            public bool WasVisited;

            public int CameFromIndex;

            public bool IsWalkable;

            public void UpdateFCost()
            {
                FCost = GCost + HCost;
            }
        }

        public void Execute()
        {
            AStar(start, end);
        }

        private void AStar(int2 start, int2 end)
        {
            NativeArray<Node> nodeArray = CreateGraph(end);
            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(4, Allocator.Temp);

            neighbourOffsetArray[0] = new int2(-1, 0); // Left
            neighbourOffsetArray[1] = new int2(+1, 0); // Right
            neighbourOffsetArray[2] = new int2(0, +1); // Up
            neighbourOffsetArray[3] = new int2(0, -1); // Down


            int endIndex = CalculateIndex(end.x, end.y, areaSize.x);

            if (walkableArray[endIndex] == true || closestAvaliable == true)
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
                        int2 neighbourPosition = new int2(currentNode.position.x + neighbourOffset.x, currentNode.position.y + neighbourOffset.y);

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

                        int2 currentNodePosition = new int2(currentNode.position.x, currentNode.position.y);

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
                path.Add(new int2(endNode.position.x, endNode.position.y));

                Node currentNode = endNode;
                while (currentNode.CameFromIndex != -1)
                {
                    Node cameFromNode = nodeArray[currentNode.CameFromIndex];
                    path.Add(new int2(cameFromNode.position.x, cameFromNode.position.y));
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

        public static int CalculateIndex(int x, int y, int gridWidth) => x + y * gridWidth;

        public static int CalculateIndex(int2 pos, int gridWidth) => pos.x + pos.y * gridWidth;

        //Manhattan Distance
        private static int CalculateHeuristicCost(int2 start, int2 end) => math.abs(start.x - end.x) + math.abs(start.y - end.y);

        private NativeArray<Node> CreateGraph(int2 end)
        {

            NativeArray<Node> nodeArray = new NativeArray<Node>(areaSize.x * areaSize.y, Allocator.Temp);
            //init
            for (int x = 0; x < areaSize.x; x++)
            {
                for (int y = 0; y < areaSize.y; y++)
                {
                    Node node = new Node();
                    node.position.x = x;
                    node.position.y = y;

                    node.index = CalculateIndex(x, y, areaSize.x);

                    node.GCost = int.MaxValue;
                    node.HCost = CalculateHeuristicCost(new int2(x, y), end);
                    node.UpdateFCost();

                    node.CameFromIndex = -1;//invalid value - meaning an encounter of the last node in the path

                    if (node.position.x == end.x && node.position.y == end.y && closestAvaliable)
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
}