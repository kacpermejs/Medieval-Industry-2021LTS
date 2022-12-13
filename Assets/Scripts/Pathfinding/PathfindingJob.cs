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
    [BurstCompile]
    public struct PathfindingJob : IJob
    {
        public int BaseEdgeCost; //for normal tiles

        public int2 Start;
        public int2 End;
        public int2 AreaSize;

        [ReadOnly]
        public NativeArray<int> WalkableArray;

        [WriteOnly]
        public NativeList<int2> Path;
        
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

        private void AStarLazy()
        {
            bool foundPath = false;
            int2 endPosition = End;

            NativeList<int2> openSet = new(Allocator.Temp);
            NativeHashSet<int2> closedSet = new(256, Allocator.Temp);
            NativeHashMap<int2, Node2> allDiscoveredNodes = new(256, Allocator.Temp);

            NativeArray<int2> neighbourOffsetArray = CreateOffsetArray();//only straight across, no diagonals

            //determine if we pathfind to exact location or the neighbour cell
            bool exactLocation = true;
            if (IsBlocked(End))
                exactLocation = false;

            Node2 startNode = GetNodeLazy(Start, allDiscoveredNodes);
            startNode.GCost = 0;

            openSet.Add(startNode.position);

            int2 currentNodePosition;
            while (openSet.Length > 0)
            {
                currentNodePosition = GetLowestFCostNodePosition(openSet, allDiscoveredNodes);

                //here node should have been already added to the hashmap
                Node2 currentNode = GetNodeLazy(currentNodePosition, allDiscoveredNodes);

                //end conditions
                if (currentNodePosition.Equals(endPosition))
                {
                    foundPath = true;
                    break;
                }
                else if (!exactLocation)
                {
                    if (IsOneCellAway(currentNode))
                    {
                        endPosition = currentNodePosition;
                        foundPath = true;
                        break;
                    }
                }

                //Remove processed node from open set...
                for (int i = 0; i < openSet.Length; i++)
                {
                    if (openSet[i].Equals(currentNodePosition))
                    {
                        openSet.RemoveAtSwapBack(i);
                        break;
                    }
                }

                //... and add it to closed set
                closedSet.Add(currentNodePosition);

                //evaluate neighbours
                for (int i = 0; i < neighbourOffsetArray.Length; i++)
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.position.x + neighbourOffset.x,
                                                      currentNode.position.y + neighbourOffset.y);

                    if (!ValidatePosition(neighbourPosition))
                        continue;
                    if (closedSet.Contains(neighbourPosition)) //node already processed
                        continue;

                    Node2 neighbourNode = GetNodeLazy(neighbourPosition, allDiscoveredNodes);
                    if (!neighbourNode.IsWalkable)
                        continue;

                    //simplification - in this implementation graph edges are all the same
                    int edgeCost = WalkableArray[CalculateIndex(neighbourPosition)];

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

            ReconstructPath(foundPath, endPosition, allDiscoveredNodes);

            //cleanup
            openSet.Dispose();
            closedSet.Dispose();
            allDiscoveredNodes.Dispose();
            neighbourOffsetArray.Dispose();
        }

        private void ReconstructPath(bool foundPath, int2 endPosition, NativeHashMap<int2, Node2> allDiscoveredNodes)
        {
            //path reconstruction
            Node2 endNode = GetNodeLazy(endPosition, allDiscoveredNodes);
            if (!foundPath)
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
        }

        private bool IsBlocked(int2 pos)
        {
            return WalkableArray[CalculateIndex(pos)] == -1;
        }

        private bool IsOneCellAway(Node2 currentNode)
        {
            int xDiff = currentNode.position.x - End.x;
            int yDiff = currentNode.position.y - End.y;
            return math.abs(xDiff) <= 1 && math.abs(yDiff) <= 1;
        }

        private static NativeArray<int2> CreateOffsetArray()
        {
            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(4, Allocator.Temp);

            neighbourOffsetArray[0] = new int2(-1, 0); // Left
            neighbourOffsetArray[1] = new int2(+1, 0); // Right
            neighbourOffsetArray[2] = new int2(0, +1); // Up
            neighbourOffsetArray[3] = new int2(0, -1); // Down
            return neighbourOffsetArray;
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

        private int2 GetLowestFCostNodePosition(NativeList<int2> openSet,
                                                NativeHashMap<int2, Node2> allDiscoveredNodes)
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
            if(allDiscoveredNodes.TryGetValue(pos, out node))
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

        private bool ValidatePosition(int2 position)
        {
            return position.x < AreaSize.x && position.y < AreaSize.y && position.x > 0 && position.y > 0;
        }

        private int CalculateIndex(int x, int y) => x + y * AreaSize.x;

        private int CalculateIndex(int2 pos) => pos.x + pos.y * AreaSize.x;

        //Manhattan Distance
        private int CalculateHeuristicCost(int2 start, int2 end)
        {
            int baseCost = ((int)ManhattanDistance(start, end)) * BaseEdgeCost;


            return baseCost;
        }

        private static float ManhattanDistance(int2 start, int2 end) => (Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y));


    }
}