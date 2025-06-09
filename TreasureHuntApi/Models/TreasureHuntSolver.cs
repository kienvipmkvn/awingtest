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
                    throw new Exception($"No coordinates found for key {pindex + 1}");
                }
            }

            //dp[k, cordinate] = minimun fuel needed to reach key k from cordinate 
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

            for (int k = 1; k <= p; k++)
            {
                foreach (var curr in cordinatesByKey[k])
                {
                    double minCost = double.MaxValue;
                    (int, int) prevCordinate = (0, 0);
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

            // Find the minimum cost at the last key
            double minTotal = double.MaxValue;
            (int x, int y) last = (0, 0);
            foreach (var kv in dp[p])
            {
                if (kv.Value.cost < minTotal)
                {
                    minTotal = kv.Value.cost;
                    last = kv.Key;
                }
            }

            // Reconstruct path
            var path = new List<(int x, int y)>();
            (int x, int y)? currPos = last;
            for (int k = p; k >= 0 && currPos != null; k--)
            {
                path.Add(currPos.Value);
                currPos = dp[k][currPos.Value].Item2;
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