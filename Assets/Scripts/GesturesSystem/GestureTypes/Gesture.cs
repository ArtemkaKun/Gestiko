using System;

namespace GesturesSystem.GestureTypes
{
	public class Gesture
	{
        private Point[] NormalizedPointsCollection { get; set; }
        private Point[] RawPointsCollection { get; set; }
        private int[][] LookUpTable { get; set; }

        public Gesture (Point[] points)
        {
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
        }
    }
	
	public class Gesture
    {
        public void Normalize(bool computeLUT = true)
        {
            this.Points = Scale(Points);
            this.Points = TranslateTo(Points, Centroid(Points));
            
            if (computeLUT) // constructs a lookup table for fast lower bounding (used by $Q)
            {
                this.TransformCoordinatesToIntegers();
                this.ConstructLUT();
            }
        }

        #region gesture pre-processing steps: scale normalization, translation to origin, and resampling

        /// <summary>
        /// Performs scale normalization with shape preservation into [0..1]x[0..1]
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private Point[] Scale(Point[] points)
        {
            float minx = float.MaxValue, miny = float.MaxValue, maxx = float.MinValue, maxy = float.MinValue;
            for (int i = 0; i < points.Length; i++)
            {
                if (minx > points[i].X) minx = points[i].X;
                if (miny > points[i].Y) miny = points[i].Y;
                if (maxx < points[i].X) maxx = points[i].X;
                if (maxy < points[i].Y) maxy = points[i].Y;
            }

            Point[] newPoints = new Point[points.Length];
            float scale = Math.Max(maxx - minx, maxy - miny);
            for (int i = 0; i < points.Length; i++)
                newPoints[i] = new Point((points[i].X - minx) / scale, (points[i].Y - miny) / scale, points[i].StrokeID);
            return newPoints;
        }

        /// <summary>
        /// Translates the array of points by p
        /// </summary>
        /// <param name="points"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private Point[] TranslateTo(Point[] points, Point p)
        {
            Point[] newPoints = new Point[points.Length];
            for (int i = 0; i < points.Length; i++)
                newPoints[i] = new Point(points[i].X - p.X, points[i].Y - p.Y, points[i].StrokeID);
            return newPoints;
        }

        /// <summary>
        /// Computes the centroid for an array of points
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private Point Centroid(Point[] points)
        {
            float cx = 0, cy = 0;
            for (int i = 0; i < points.Length; i++)
            {
                cx += points[i].X;
                cy += points[i].Y;
            }
            return new Point(cx / points.Length, cy / points.Length, 0);
        }
        
        /// <summary>
        /// Scales point coordinates to the integer domain [0..MAXINT-1] x [0..MAXINT-1]
        /// </summary>
        private void TransformCoordinatesToIntegers()
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].intX = (int)((Points[i].X + 1.0f) / 2.0f * (MAX_INT_COORDINATES - 1));
                Points[i].intY = (int)((Points[i].Y + 1.0f) / 2.0f * (MAX_INT_COORDINATES - 1));
            }
        }

        /// <summary>
        /// Constructs a Lookup Table that maps grip points to the closest point from the gesture path
        /// </summary>
        private void ConstructLUT()
        {
            this.LUT = new int[LUT_SIZE][];
            for (int i = 0; i < LUT_SIZE; i++)
                LUT[i] = new int[LUT_SIZE];

            for (int i = 0; i < LUT_SIZE; i++)
                for (int j = 0; j < LUT_SIZE; j++)
                {
                    int minDistance = int.MaxValue;
                    int indexMin = -1;
                    for (int t = 0; t < Points.Length; t++)
                    {
                        int row = Points[t].intY / LUT_SCALE_FACTOR;
                        int col = Points[t].intX / LUT_SCALE_FACTOR;
                        int dist = (row - i) * (row - i) + (col - j) * (col - j);
                        if (dist < minDistance)
                        {
                            minDistance = dist;
                            indexMin = t;
                        }
                    }
                    LUT[i][j] = indexMin;
                }
        }

        #endregion
    }
}