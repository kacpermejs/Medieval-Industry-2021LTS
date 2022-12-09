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

        public int2 Start;
        public int2 End;
        public int2 AreaSize;
        //public bool closestAvaliable;

        [ReadOnly]
        public NativeArray<int> WalkableArray;

        public NativeList<int2> Path;

        //these are from normal implementation
        //private NativeList<int> _openList;
        //private NativeList<int> _closedList;


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
        
        public struct Node2
        {
            public int2 position;

            public int GCost;
            public int HCost;
            public int FCost => GCost + HCost;

            public bool WasVisited;

            public int2 CameFrom;

            public bool IsWalkable;
        }

        public void Execute()
        {
            AStarLazy();
        }

        #region OLD
        /*
        private void AStar_old(int2 start, int2 end)
        {
            NativeArray<Node> nodeArray = CreateGraph(end);
            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(4, Allocator.Temp);

            neighbourOffsetArray[0] = new int2(-1, 0); // Left
            neighbourOffsetArray[1] = new int2(+1, 0); // Right
            neighbourOffsetArray[2] = new int2(0, +1); // Up
            neighbourOffsetArray[3] = new int2(0, -1); // Down


            int endIndex = CalculateIndex(end.x, end.y);

            if (WalkableArray[endIndex] > 0 ) // || closestAvaliable == true)
            {


                Node startNode = nodeArray[CalculateIndex(start.x, start.y)];
                startNode.GCost = 0;
                startNode.UpdateFCost();
                nodeArray[startNode.index] = startNode;

                _openList = new NativeList<int>(Allocator.Temp);
                _closedList = new NativeList<int>(Allocator.Temp);
                

                _openList.Add(startNode.index);

                int currentNodeIndex;
                while (_openList.Length > 0)
                {
                    currentNodeIndex = GetLowestFCostNodeIndex(_openList, nodeArray);

                    Node currentNode = nodeArray[currentNodeIndex];

                    if (currentNodeIndex == endIndex)
                    {
                        break;
                    }

                    for (int i = 0; i < _openList.Length; i++)
                    {
                        if (_openList[i] == currentNodeIndex)
                        {
                            _openList.RemoveAtSwapBack(i);
                            break;
                        }
                    }

                    _closedList.Add(currentNodeIndex);

                    for (int i = 0; i < neighbourOffsetArray.Length; i++)
                    {
                        int2 neighbourOffset = neighbourOffsetArray[i];
                        int2 neighbourPosition = new int2(currentNode.position.x + neighbourOffset.x, currentNode.position.y + neighbourOffset.y);

                        if (!ValidatePosition(neighbourPosition, AreaSize))
                        {
                            continue;
                        }

                        int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y);

                        if (_closedList.Contains(neighbourNodeIndex))
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

                            if (!_openList.Contains(neighbourNode.index))
                            {
                                _openList.Add(neighbourNode.index);
                            }
                        }
                    }
                }
                _openList.Dispose();
                _closedList.Dispose();
            }

            Node endNode = nodeArray[endIndex];
            if (endNode.CameFromIndex == -1)
            {
                //no path in range
                Debug.Log("Didn't find a path!");
                CalculatePath(start, nodeArray, endNode, Path);
            }
            else
            {
                //found a path
                CalculatePath(start, nodeArray, endNode, Path);
            }

            
            neighbourOffsetArray.Dispose();
            nodeArray.Dispose();
        }

        private void AStar(int2 start, int2 end)
        {
            NativeArray<Node> nodeArray = CreateGraph(end);
            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(4, Allocator.Temp);

            neighbourOffsetArray[0] = new int2(-1, 0); // Left
            neighbourOffsetArray[1] = new int2(+1, 0); // Right
            neighbourOffsetArray[2] = new int2(0, +1); // Up
            neighbourOffsetArray[3] = new int2(0, -1); // Down


            int endIndex = CalculateIndex(end.x, end.y);

            bool exactLocation = true;
            if (WalkableArray[endIndex] <= 0 )
            {
                exactLocation = false;
            }
            Node startNode = nodeArray[CalculateIndex(start.x, start.y)];
            startNode.GCost = 0;
            startNode.UpdateFCost();
            nodeArray[startNode.index] = startNode;

            _openList = new NativeList<int>(Allocator.Temp);
            _closedList = new NativeList<int>(Allocator.Temp);


            _openList.Add(startNode.index);

            int currentNodeIndex;
            while (_openList.Length > 0)
            {
                currentNodeIndex = GetLowestFCostNodeIndex(_openList, nodeArray);

                Node currentNode = nodeArray[currentNodeIndex];

                if (currentNodeIndex == endIndex)
                {
                    break;
                }
                else if(!exactLocation)
                {
                    int xDiff = currentNode.position.x - end.x;
                    int yDiff = currentNode.position.y - end.y;
                    if( math.abs(xDiff) <= 1 && math.abs(yDiff) <= 1 )
                    {
                        endIndex = currentNodeIndex;
                        break;
                    }
                }

                for (int i = 0; i < _openList.Length; i++)
                {
                    if (_openList[i] == currentNodeIndex)
                    {
                        _openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                _closedList.Add(currentNodeIndex);

                for (int i = 0; i < neighbourOffsetArray.Length; i++)
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.position.x + neighbourOffset.x, currentNode.position.y + neighbourOffset.y);

                    if (!ValidatePosition(neighbourPosition, AreaSize))
                    {
                        continue;
                    }

                    int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y);

                    if (_closedList.Contains(neighbourNodeIndex))
                    {
                        continue;
                    }

                    Node neighbourNode = nodeArray[neighbourNodeIndex];
                    if (!neighbourNode.IsWalkable)
                    {
                        continue;
                    }

                    int2 currentNodePosition = new int2(currentNode.position.x, currentNode.position.y);

                    float modifier = (WalkableArray[neighbourNodeIndex]) * 0.01f;

                    int edgeCost = CalculateHeuristicCost(currentNodePosition, neighbourPosition);

                    float discount = edgeCost * modifier;

                    edgeCost -= (int)discount;

                    //Debug.Log(modifier + " -> discount -> " + discount);
                        


                    int tentativeGCost = currentNode.GCost + edgeCost;
                    if (tentativeGCost < neighbourNode.GCost)
                    {
                        neighbourNode.CameFromIndex = currentNodeIndex;
                        neighbourNode.GCost = tentativeGCost;
                        neighbourNode.UpdateFCost();
                        nodeArray[neighbourNodeIndex] = neighbourNode;

                        if (!_openList.Contains(neighbourNode.index))
                        {
                            _openList.Add(neighbourNode.index);
                        }
                    }
                }
            }
            _openList.Dispose();
            _closedList.Dispose();
            

            Node endNode = nodeArray[endIndex];
            if (endNode.CameFromIndex == -1)
            {
                //no path in range
                Debug.Log("Didn't find a path!");
                CalculatePath(start, nodeArray, endNode, Path);
            }
            else
            {
                //found a path
                CalculatePath(start, nodeArray, endNode, Path);
            }


            neighbourOffsetArray.Dispose();
            nodeArray.Dispose();
        }
        */
        #endregion
        private void AStarLazy()
        {
            bool foundPath = false;

            NativeList<int2> openSet = new(Allocator.Temp);
            NativeList<int2> closedSet = new(Allocator.Temp);

            NativeHashMap<int2, Node2> allDiscoveredNodes = new(64, Allocator.Temp);

            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(4, Allocator.Temp);

            neighbourOffsetArray[0] = new int2(-1, 0); // Left
            neighbourOffsetArray[1] = new int2(+1, 0); // Right
            neighbourOffsetArray[2] = new int2(0, +1); // Up
            neighbourOffsetArray[3] = new int2(0, -1); // Down


            int endIndex = CalculateIndex(End.x, End.y);
            int2 endPosition = End;

            bool exactLocation = true;
            if (WalkableArray[endIndex] <= 0 )
            {
                exactLocation = false;
            }

            Node2 startNode = GetNodeLazy(Start, allDiscoveredNodes);

            startNode.GCost = 0;            
            
            
            openSet.Add(startNode.position);

            int2 currentNodePosition;
            while (openSet.Length > 0)
            {
                currentNodePosition = GetLowestFCostNodePosition(openSet, allDiscoveredNodes);

                Node2 currentNode = GetNodeLazy(currentNodePosition, allDiscoveredNodes);

                if (currentNodePosition.x == End.x && currentNodePosition.y == End.y)
                {
                    foundPath = true;
                    break;
                }
                else if(!exactLocation)
                {
                    int xDiff = currentNode.position.x - End.x;
                    int yDiff = currentNode.position.y - End.y;
                    if( math.abs(xDiff) <= 1 && math.abs(yDiff) <= 1 )
                    {
                        endPosition = currentNodePosition;
                        foundPath = true;
                        break;
                    }
                }

                for (int i = 0; i < openSet.Length; i++)
                {
                    if (openSet[i].x == currentNodePosition.x && openSet[i].y == currentNodePosition.y)
                    {
                        openSet.RemoveAtSwapBack(i);
                        break;
                    }
                }

                closedSet.Add(currentNodePosition);

                for (int i = 0; i < neighbourOffsetArray.Length; i++)
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.position.x + neighbourOffset.x, currentNode.position.y + neighbourOffset.y);

                    if (!ValidatePosition(neighbourPosition, AreaSize))
                    {
                        continue;
                    }

                    if (closedSet.Contains(neighbourPosition))
                    {
                        continue;
                    }

                    Node2 neighbourNode = GetNodeLazy(neighbourPosition, allDiscoveredNodes);
                    if (!neighbourNode.IsWalkable)
                    {
                        continue;
                    }

                    int edgeCost = CalculateHeuristicCost(currentNodePosition, neighbourPosition);

                    int tentativeGCost = currentNode.GCost + edgeCost;
                    if (tentativeGCost < neighbourNode.GCost)
                    {
                        neighbourNode.CameFrom = currentNodePosition;
                        neighbourNode.GCost = tentativeGCost;
                        allDiscoveredNodes[neighbourNode.position] = neighbourNode;

                        if (!openSet.Contains(neighbourNode.position))
                        {
                            openSet.Add(neighbourNode.position);
                        }
                    }
                }
            }
            

            Node2 endNode = GetNodeLazy(endPosition, allDiscoveredNodes);
            if ( !foundPath )
            {
                //no path 
                Debug.Log("Didn't find a path!");
                Path.Add(Start);
            }
            else
            {
                //found a path
                CalculatePath(endNode, allDiscoveredNodes);
            }


            openSet.Dispose();
            closedSet.Dispose();
            allDiscoveredNodes.Dispose();
            neighbourOffsetArray.Dispose();
        }

        private void CalculatePath(Node2 endNode, NativeHashMap<int2, Node2> allDiscoveredNodes)
        {
            
            Path.Add(new int2(endNode.position.x, endNode.position.y));

            Node2 currentNode = endNode;
            while ( true )
            {
                if (currentNode.CameFrom.x == Start.x && currentNode.CameFrom.y == Start.y)
                    break;

                Node2 cameFromNode = allDiscoveredNodes[currentNode.CameFrom];
                Path.Add(new int2(cameFromNode.position.x, cameFromNode.position.y));
                currentNode = cameFromNode;
            }
            
        }

        private int2 GetLowestFCostNodePosition(NativeList<int2> openSet, NativeHashMap<int2, Node2> allDiscoveredNodes)
        {
            Node2 lowestCostNode = GetNodeLazy(openSet[0], allDiscoveredNodes);
            for (int i = 0; i < openSet.Length; i++)
            {
                Node2 nodeToCheck = GetNodeLazy(openSet[i], allDiscoveredNodes);

                if (nodeToCheck.FCost < lowestCostNode.FCost)
                {
                    lowestCostNode = nodeToCheck;
                }
            }
            return lowestCostNode.position;
        }

        private Node2 GetNodeLazy(int2 pos, NativeHashMap<int2, Node2> allDiscoveredNodes)
        {
            Node2 node;
            if(allDiscoveredNodes.ContainsKey(pos))
            {
                node = allDiscoveredNodes[pos];
            }
            else
            {
                node = new Node2();
                node.position = pos;

                node.GCost = int.MaxValue;
                node.HCost = CalculateHeuristicCost(pos, End);

                node.CameFrom = Start;

                node.IsWalkable = WalkableArray[CalculateIndex(pos.x, pos.y)] > 0;
            }

            return node;
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

        public int CalculateIndex(int x, int y) => x + y * AreaSize.x;

        public int CalculateIndex(int2 pos) => pos.x + pos.y * AreaSize.x;

        //Manhattan Distance
        private static int CalculateHeuristicCost(int2 start, int2 end) => ((int)ManhattanDistance(start, end)) * STRAIGHT_COST;

        private static float EuclideanDistance(int2 start, int2 end) => Mathf.Sqrt(Mathf.Pow(start.x - end.x, 2) + Mathf.Pow(start.y - end.y, 2));

        private static float ManhattanDistance(int2 start, int2 end) => (Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y));

        

        private NativeArray<Node> CreateGraph(int2 end)
        {

            NativeArray<Node> nodeArray = new NativeArray<Node>(AreaSize.x * AreaSize.y, Allocator.Temp);
            //init
            for (int x = 0; x < AreaSize.x; x++)
            {
                for (int y = 0; y < AreaSize.y; y++)
                {
                    Node node = new Node();
                    node.position.x = x;
                    node.position.y = y;

                    node.index = CalculateIndex(x, y);

                    node.GCost = int.MaxValue;
                    node.HCost = CalculateHeuristicCost(new int2(x, y), end);
                    node.UpdateFCost();

                    node.CameFromIndex = -1;//invalid value - meaning an encounter of the last node in the path


                    /*if (node.position.x == end.x && node.position.y == end.y && closestAvaliable)
                    {
                        node.IsWalkable = true;
                    }
                    else
                    {
                        node.IsWalkable = walkableArray[node.index] > 0;
                    }*/
                    node.IsWalkable = WalkableArray[node.index] > 0;

                    nodeArray[node.index] = node;
                }
            }
            return nodeArray;
        }


    }
}