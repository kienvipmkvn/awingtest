using System.Collections.Generic;

namespace TreasureHuntApi.Models
{
    public static class TreasureHuntSolver
    {
        public static (double minFuel, List<(int x, int y)> path) Solve(TreasureHuntInput input)
        {
            int n = input.N;
            int m = input.M;
            int p = input.P;
            int[][] matrix = input.Matrix;


            Dictionary<int, List<(int x, int y)>> cordinatesByKey = new Dictionary<int, List<(int x, int y)>>();
            cordinatesByKey[0] = new List<(int x, int y)>() { (0, 0) };

            for (int pindex = 1; pindex <= p; pindex++)
            {
                cordinatesByKey[pindex] = new List<(int x, int y)>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (matrix[i][j] == pindex)
                        {
                            cordinatesByKey[pindex].Add((i, j));
                        }
                    }
                }

                if (cordinatesByKey[pindex].Count == 0)
                {
                    throw new Exception($"No coordinates found for key {pindex}");
                }
            }

            if (cordinatesByKey[p].Count > 1)
            {
                throw new Exception($"Too many key {p}");
            }

            //dp[k][cordinate] = minimun fuel needed to reach key k at cordinate 
            var dp = new List<Dictionary<(int x, int y), (double cost, (int prevX, int prevY)?)>>();

            for (int k = 0; k <= p; k++)
            {
                dp.Add([]);
            }

            dp[0][(0, 0)] = (0, null);

            // key 0 can open chest number 1
            // calculate the fuel needed to go from (0, 0) to chest 1 (key 0)
            foreach (var cordinate in cordinatesByKey[1])
            {
                dp[1][cordinate] = (Distance((0, 0), cordinate), (0, 0));
            }

            for (int k = 2; k <= p; k++)
            {
                foreach (var curr in cordinatesByKey[k])
                {
                    double minCost = double.MaxValue;
                    (int, int)? prevCordinate = null;
                    foreach (var prev in cordinatesByKey[k - 1])
                    {
                        if (!dp[k - 1].ContainsKey(prev))
                        {
                            continue;
                        }

                        double cost = dp[k - 1][prev].cost + Distance(prev, curr);
                        if(cost < minCost)
                        {
                            minCost = cost;
                            prevCordinate = prev;
                        }
                    }
                    if(minCost < double.MaxValue)
                    {
                        dp[k][curr] = (minCost, prevCordinate);
                    }
                }
            }

            // Reconstruct path
            var path = new List<(int x, int y)>();
            (int x, int y) lastCordinate = cordinatesByKey[p][0];
            (int x, int y)? currCordinate = lastCordinate;
            var minTotal = dp[p][lastCordinate].cost;
            for (int k = p; k >= 0; k--)
            {
                path.Add(currCordinate.Value);
                currCordinate = dp[k][currCordinate.Value].Item2;
            }
            path.Reverse();

            return (minTotal, path);
        }

        private static double Distance((int x, int y) a, (int x, int y) b)
        {
            return Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
        }
    }
} 