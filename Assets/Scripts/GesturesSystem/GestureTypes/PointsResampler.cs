using System;
using Unity.Mathematics;

namespace GesturesSystem.GestureTypes
{
	public class PointsResampler
	{
		private const int NUMBER_OF_NORMALIZED_POINTS = GestureDatabase.DEFAULT_NUMBER_OF_POINTS_IN_GESTURE;
		private const int NUMBER_OF_NORMALIZED_POINTS_MINUS_ONE = NUMBER_OF_NORMALIZED_POINTS - 1;
        
        private Point[] ResampledPointsCollection { get; set; } = new Point[NUMBER_OF_NORMALIZED_POINTS];
        private int ResampledPointIndex { get; set; } = 1;
		
        public Point[] ResamplePoints(Point[] rawPointsCollection)
        {
            ResampledPointsCollection[0] = rawPointsCollection[0];
            ProceedRawPointsData(rawPointsCollection);
            VerifyMissingLastPoint(rawPointsCollection);

            return ResampledPointsCollection;
        }

        private void ProceedRawPointsData (Point[] rawPointsCollection)
        {
            float pathLength = CalculatePathLength(rawPointsCollection) / NUMBER_OF_NORMALIZED_POINTS_MINUS_ONE;
            float gestureDistance = 0;

            for (int i = 1; i < rawPointsCollection.Length; i++)
            {
                Point cachedPoint = rawPointsCollection[i];
                Point previousPoint = rawPointsCollection[i - 1];

                if (CheckIfPointsHaveSameID(cachedPoint, previousPoint) == true)
                {
                    float distanceBetweenCurrentAndPreviousPoints = CalculateDistanceBetweenTwoPoints(previousPoint, cachedPoint);

                    if (gestureDistance + distanceBetweenCurrentAndPreviousPoints >= pathLength)
                    {
                        Point firstPoint = previousPoint;

                        while (gestureDistance + distanceBetweenCurrentAndPreviousPoints >= pathLength)
                        {
                            float2 normalizationHighestBounds = new float2(0.0f, 1.0f);
                            float normalizationCoefficient = Math.Min(Math.Max((pathLength - gestureDistance) / distanceBetweenCurrentAndPreviousPoints, normalizationHighestBounds.x), normalizationHighestBounds.y);

                            if (float.IsNaN(normalizationCoefficient) == true)
                            {
                                normalizationCoefficient = 0.5f;
                            }

                            ResampledPointsCollection[ResampledPointIndex] = new Point(cachedPoint.ID, new float2((normalizationHighestBounds.y - normalizationCoefficient) * firstPoint.Position.x + normalizationCoefficient * cachedPoint.Position.x, (normalizationHighestBounds.y - normalizationCoefficient) * firstPoint.Position.y + normalizationCoefficient * cachedPoint.Position.y));
                            distanceBetweenCurrentAndPreviousPoints = gestureDistance + distanceBetweenCurrentAndPreviousPoints - pathLength;
                            gestureDistance = 0;
                            firstPoint = ResampledPointsCollection[ResampledPointIndex];
                            ResampledPointIndex += 1;
                        }

                        gestureDistance = distanceBetweenCurrentAndPreviousPoints;
                    }
                    else
                    {
                        gestureDistance += distanceBetweenCurrentAndPreviousPoints;
                    }
                }
            }
        }

        private float CalculatePathLength(Point[] points)
        {
            float length = 0;

            for (int i = 1; i < points.Length; i++)
            {
                Point cachedPoint = points[i];
                Point previousPoint = points[i - 1];
                
                if (CheckIfPointsHaveSameID(cachedPoint, previousPoint) == true)
                {
                    length += CalculateDistanceBetweenTwoPoints(previousPoint, cachedPoint);
                }
            }

            return length;
        }

        private bool CheckIfPointsHaveSameID (Point firstPoint, Point secondPoint)
        {
            return firstPoint.ID == secondPoint.ID;
        }

        private float CalculateDistanceBetweenTwoPoints (Point firstPoint, Point secondPoint)
        {
            return math.distancesq(firstPoint.Position, secondPoint.Position);
        }

        private void VerifyMissingLastPoint (Point[] rawPointsCollection)
        {
            if (ResampledPointIndex == NUMBER_OF_NORMALIZED_POINTS_MINUS_ONE)
            {
                ResampledPointsCollection[ResampledPointIndex] = rawPointsCollection[^1];
            }
        }
    }
}