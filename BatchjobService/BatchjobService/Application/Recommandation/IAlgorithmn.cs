using BatchjobService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Recommandation
{
    public interface IAlgorithmn
    {
        double[,] run(AlgorithmData data);
        void caculateAve(AlgorithmData data);
        double[,] caculateSimilarity(AlgorithmData data);
        double cosineSimilarity(double[] vectorA, double[] vectorB);
        double[] getCol(int index, AlgorithmData data);
        double[] getColInMatrix(int index, double[,] data, int size);
        int[] ParserUserToInt(List<Users> users);
        List<suggestion> MostSimilarity(int limit, AlgorithmData algorithmData, double[,] data);

        int[] sortWeight(int[] users, double[] weight, int limit);

    }
}
