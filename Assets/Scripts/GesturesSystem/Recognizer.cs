using System;
using GesturesSystem.GestureTypes;
using Unity.Mathematics;

namespace GesturesSystem
{
	public class Recognizer
	{
		public string Classify (Gesture candidate, Gesture[] templateSet)
		{
			float minDistance = float.MaxValue;
			string gestureClass = "";

			foreach (Gesture template in templateSet)
			{
				float dist = GreedyCloudMatch(candidate, template, minDistance);

				if (dist < minDistance)
				{
					minDistance = dist;
					gestureClass = template.Name;
				}
			}

			return gestureClass;
		}

		private float GreedyCloudMatch (Gesture gesture1, Gesture gesture2, float minSoFar)
		{
			int n = gesture1.NormalizedPointsCollection.Length;
			const float eps = 0.5f;
			int step = (int)Math.Floor(Math.Pow(n, 1.0f - eps));
			float[] LB1 = ComputeLowerBound(gesture1.NormalizedPointsCollection, gesture2.NormalizedPointsCollection, gesture2.LookUpTable, step);
			float[] LB2 = ComputeLowerBound(gesture2.NormalizedPointsCollection, gesture1.NormalizedPointsCollection, gesture1.LookUpTable, step);

			for (int i = 0, indexLB = 0; i < n; i += step, indexLB++)
			{
				if (LB1[indexLB] < minSoFar)
				{
					minSoFar = Math.Min(minSoFar, CloudDistance(gesture1.NormalizedPointsCollection, gesture2.NormalizedPointsCollection, i, minSoFar));
				}

				if (LB2[indexLB] < minSoFar)
				{
					minSoFar = Math.Min(minSoFar, CloudDistance(gesture2.NormalizedPointsCollection, gesture1.NormalizedPointsCollection, i, minSoFar));  
				}
			}

			return minSoFar;
		}

		private float[] ComputeLowerBound (Point[] points1, Point[] points2, int[][] LUT, int step)
		{
			int n = points1.Length;
			float[] LB = new float[n / step + 1];
			float[] SAT = new float[n];

			LB[0] = 0;

			for (int i = 0; i < n; i++)
			{
				int2 scaledTableCoordinates = points1[i].LookUpTablePosition / GestureDatabase.LOOK_UP_TABLE_SCALE_FACTOR;
				int index = LUT[scaledTableCoordinates.y][scaledTableCoordinates.x];
				float dist = math.distancesq(points1[i].Position, points2[index].Position);
				SAT[i] = i == 0 ? dist : SAT[i - 1] + dist;
				LB[0] += (n - i) * dist;
			}

			for (int i = step, indexLB = 1; i < n; i += step, indexLB++)
			{
				LB[indexLB] = LB[0] + i * SAT[n - 1] - n * SAT[i - 1];
			}

			return LB;
		}

		private float CloudDistance (Point[] points1, Point[] points2, int startIndex, float minSoFar)
		{
			int n = points1.Length;
			int[] indexesNotMatched = new int[n];

			for (int j = 0; j < n; j++)
			{
				indexesNotMatched[j] = j;
			}

			float sum = 0;
			int i = startIndex;
			int weight = n;
			int indexNotMatched = 0;

			do
			{
				int index = -1;
				float minDistance = float.MaxValue;

				for (int j = indexNotMatched; j < n; j++)
				{
					float dist = math.distancesq(points1[i].Position, points2[indexesNotMatched[j]].Position);

					if (dist < minDistance)
					{
						minDistance = dist;
						index = j;
					}
				}

				indexesNotMatched[index] = indexesNotMatched[indexNotMatched];
				sum += (weight--) * minDistance;

				if (sum >= minSoFar)
				{
					return sum;
				}

				i = (i + 1) % n;
				indexNotMatched++;
			} while (i != startIndex);

			return sum;
		}
	}
}