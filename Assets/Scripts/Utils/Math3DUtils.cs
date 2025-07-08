
using UnityEngine;

namespace AnalyticalApproach
{
    internal static class Math3DUtils
    {

        public static Vector3 ReflectionOnPlane(Vector3 point, Vector3 planeNormal, Vector3 pointOnPlane)
        {
            planeNormal.Normalize();

            Vector3 pointToPlane = point - pointOnPlane;

            float distanceFromPlane = Vector3.Dot(pointToPlane, planeNormal);

            Vector3 reflectedPoint = point - 2 * distanceFromPlane * planeNormal;

            return reflectedPoint;
        }

        public static Vector3 ClosestPointOnPlane(Vector3 point, Vector3 planeNormal, Vector3 pointOnPlane)
        {
            // Ensure the plane normal is normalized
            planeNormal.Normalize();

            // Calculate the vector from the point to the plane
            Vector3 pointToPlane = point - pointOnPlane;

            // Project the vector onto the plane normal to get the distance
            float distanceFromPlane = Vector3.Dot(pointToPlane, planeNormal);

            // Subtract this distance from the original point to get the closest point on the plane
            Vector3 closestPoint = point - distanceFromPlane * planeNormal;

            return closestPoint;
        }


        public static float AngleBetweenTwoVectors(Vector3 vectorA, Vector3 vectorB)
        {

            vectorA.Normalize();
            vectorB.Normalize();

            Vector3 orthogonalAxis = Vector3.Cross(vectorA.normalized, vectorB.normalized);

            if (orthogonalAxis == Vector3.zero)
            {
                Debug.LogWarning("The vectors are parallel or anti-parallel, resulting in an undefined orthogonal axis.");
                return 0;
            }


            float signedAngle = Vector3.SignedAngle(vectorA, vectorB, orthogonalAxis.normalized);

            return signedAngle;
        }
    }

}
