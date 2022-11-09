using Assets.Scripts.Pathfinding;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Pathfinding
{
    public class PathfindingScheduler : MonoBehaviour
    {

        private Queue<PathfindingJob> _pathfindingJobsQueue = new Queue<PathfindingJob>();
        private NativeList<JobHandle> _jobHandleList = new NativeList<JobHandle>(Allocator.Temp);

        private void Update()
        {
        }

        private void OnDestroy()
        {
            _jobHandleList.Dispose();
        }

        public void RequestPathfindingJob(Vector3Int startPos, Vector3Int endPos, int range, NativeList<int2> resultPath)
        {
            resultPath.Clear();
            PathfindingJob job = PathfindingJobFactory.CreatePathfindingJob(startPos, endPos, range, true, resultPath);

            _pathfindingJobsQueue.Enqueue(job);
            _jobHandleList.Add(_pathfindingJobsQueue.Dequeue().Schedule());
        }


    }
}
