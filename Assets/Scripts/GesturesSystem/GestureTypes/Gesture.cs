using System;
using Unity.Mathematics;

namespace GesturesSystem.GestureTypes
{
	public class Gesture
	{
        public string Name { get; private set; }
        
        private Point[] NormalizedPointsCollection { get; set; }
        private Point[] RawPointsCollection { get; set; }
        private int[][] LookUpTable { get; set; }

        public Gesture (Point[] points, string gestureName)
        {
            Name = gestureName;
            ProceedInputPoints(points);
        }

        private void ProceedInputPoints (Point[] points)
        {
            FillRawPointCollection(points);
            NormalizePoints();
        }

        private void FillRawPointCollection (Point[] points)
        {
            int sourceArrayLength = points.Length;
            RawPointsCollection = new Point[sourceArrayLength];
            Array.Copy(points, RawPointsCollection, sourceArrayLength);
        }

        private void NormalizePoints ()
        {
            NormalizedPointsCollection = new PointsResampler().ResamplePoints(RawPointsCollection);
            NormalizedPointsCollection = ScaleNormalizedPoints(NormalizedPointsCollection);
            NormalizedPointsCollection = TranslateTo(NormalizedPointsCollection, Centroid(NormalizedPointsCollection));
            TransformCoordinatesToIntegers();
            ConstructLUT();
        }
        
        private Point[] ScaleNormalizedPoints(Point[] points)
        {
            float minx = float.MaxValue;
            float miny = float.MaxValue;
            float maxx = float.MinValue;
            float maxy = float.MinValue;
            
            for (int i = 0; i < points.Length; i++)
            {
                float2 currentPointPosition = points[i].Position;
                minx = math.min(minx, currentPointPosition.x);
                miny = math.min(miny, currentPointPosition.y);
                maxx = math.max(maxx, currentPointPosition.x);
                maxy = math.max(maxy, currentPointPosition.y);
            }

            Point[] newPoints = new Point[points.Length];
            float scale = Math.Max(maxx - minx, maxy - miny);
            
            for (int i = 0; i < points.Length; i++)
            {
                float2 currentPointPosition = points[i].Position;
                newPoints[i] = new Point(points[i].ID, (currentPointPosition - new float2(minx, miny)) / scale);
            }

            return newPoints;
        }
        
        private Point[] TranslateTo(Point[] points, Point p)
        {
            Point[] newPoints = new Point[points.Length];
            
            for (int i = 0; i < points.Length; i++)
            {
                newPoints[i] = new Point( points[i].ID, points[i].Position - p.Position);
            }

            return newPoints;
        }
        
        private Point Centroid(Point[] points)
        {
            float2 centroidCoordinates = float2.zero;
            
            for (int i = 0; i < points.Length; i++)
            {
                centroidCoordinates += points[i].Position;
            }
            
            return new Point(0, centroidCoordinates / points.Length);
        }
        
        private void TransformCoordinatesToIntegers()
        {
            for (int i = 0; i < NormalizedPointsCollection.Length; i++)
            {
                NormalizedPointsCollection[i].SetLookUpTablePosition((int2)((NormalizedPointsCollection[i].Position + 1.0f) / 2.0f * (GestureDatabase.MAX_LOOK_UP_TABLE_COORDINATE - 1)));
            }
        }
        
        private void ConstructLUT()
        {
            LookUpTable = new int[GestureDatabase.DEFAULT_LOOK_UP_TABLE_SIZE][];
            
            for (int i = 0; i < LookUpTable.Length; i++)
            {
                LookUpTable[i] = new int[LookUpTable.Length];
            }

            for (int i = 0; i < LookUpTable.Length; i++)
            {
                for (int j = 0; j < LookUpTable.Length; j++)
                {
                    int minDistance = int.MaxValue;
                    int indexMin = -1;
                    
                    for (int t = 0; t < NormalizedPointsCollection.Length; t++)
                    {
                        int2 tableCellPosition = NormalizedPointsCollection[t].LookUpTablePosition / GestureDatabase.LOOK_UP_TABLE_SCALE_FACTOR;
                        int dist = (tableCellPosition.y - i) * (tableCellPosition.y - i) + (tableCellPosition.x - j) * (tableCellPosition.x - j);
                        
                        if (dist < minDistance)
                        {
                            minDistance = dist;
                            indexMin = t;
                        }
                    }
                    
                    LookUpTable[i][j] = indexMin;
                }
            }
        }
    }
}