using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace JUTPS.AI
{
    public class JUPathFinder
    {
        /// <summary>
        /// Calculates the path from the source position to a target position. It is necessary to bake NavMesh in the scene.
        /// </summary>
        /// <param name="SourcePosition">Source position</param>
        /// <param name="TargetPosition">Target position</param>
        /// <returns></returns>
        public static Vector3[] CalculatePath(Vector3 SourcePosition, Vector3 TargetPosition)
        {
            //Calculate path
            NavMeshPath navmesh_path = new NavMeshPath();
            NavMesh.CalculatePath(SourcePosition, TargetPosition, NavMesh.AllAreas, navmesh_path);

            //If cannot do pathfinding with current position, create a path with the closest navmesh edge
            if (navmesh_path.status == NavMeshPathStatus.PathInvalid)
            {
                NavMeshHit hit;
                NavMesh.SamplePosition(SourcePosition, out hit, 100, NavMesh.AllAreas);
                //Could not calculate path
                if (!hit.hit) { Debug.LogWarning("Could not calculate NavMesh path, invalid target/source position"); return new Vector3[0] { }; }
                NavMesh.CalculatePath(hit.position, TargetPosition, NavMesh.AllAreas, navmesh_path);
            }
            //If still invalid
            if (navmesh_path.status == NavMeshPathStatus.PathInvalid)
            {
                //Get valid source position
                NavMeshHit hit;
                NavMesh.SamplePosition(SourcePosition, out hit, 100, NavMesh.AllAreas);
                //Get valid END position
                NavMeshHit hitEnd;
                NavMesh.SamplePosition(TargetPosition, out hitEnd, 100, NavMesh.AllAreas);

                if (!hit.hit || !hitEnd.hit) { Debug.LogWarning("Could not calculate NavMesh path, invalid target/source position"); return new Vector3[0] { }; }

                //Set position
                NavMesh.CalculatePath(hit.position, hitEnd.position, NavMesh.AllAreas, navmesh_path);
            }


            //Get path
            Vector3[] Path = navmesh_path.corners;

            //Return the path
            return Path;
        }
        /// <summary>
        /// Calculates the path from the source position to a target position. It is necessary to bake NavMesh in the scene.
        /// </summary>
        /// <param name="SourcePosition">Source position</param>
        /// <param name="TargetPosition">Target position</param>
        /// <returns></returns>
        public static Vector3[] CalculatePath(Transform SourcePosition, Transform TargetPosition)
        {
            //Calculate path
            NavMeshPath navmesh_path = new NavMeshPath();
            NavMesh.CalculatePath(SourcePosition.position, TargetPosition.position, NavMesh.AllAreas, navmesh_path);

            //If cannot do pathfinding with current position, create a path with the closest navmesh edge
            if(navmesh_path.status == NavMeshPathStatus.PathInvalid)
            {
                NavMeshHit hit;
                NavMesh.FindClosestEdge(SourcePosition.position, out hit, NavMesh.AllAreas);
                NavMesh.CalculatePath(hit.position, TargetPosition.position, NavMesh.AllAreas, navmesh_path);
            }

            //Get path
            Vector3[] Path = navmesh_path.corners;

            //Return the path
            return Path;
        }

        /// <summary>
        /// Draw the path by lines
        /// </summary>
        /// <param name="path">path to draw</param>
        public static void VisualizePath(Vector3[] path)
        {
            Color color = Color.white;
            color.a = 0.2f;
            for (int i = 0; i < path.Length - 1; i++)
            {
                Debug.DrawLine(path[i], path[i] + Vector3.up * 0.1f, Color.red);
                Debug.DrawLine(path[i], path[i + 1], color);
            }
        }


        public static Vector3 GetClosestWalkablePoint(Vector3 targetPosition, float offsetDirection = 0.2f)
        {
            Vector3 position = Vector3.zero;

            NavMeshHit hit;
            NavMesh.SamplePosition(targetPosition, out hit, 100, NavMesh.AllAreas);

            Vector3 dir = (targetPosition - hit.position).normalized;
            position = hit.position - dir * offsetDirection;

            return position;
        }
    }
}