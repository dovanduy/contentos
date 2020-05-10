using BatchjobService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Recommandation
{
    public class Algorithmn : IAlgorithmn
    {
        static public double NULL = Double.MinValue;


        public double[,] run(AlgorithmData data)
        {
            double[,] rs;
            caculateAve(data);
            //set 0 weight with null value
            for (int i = 0; i < data.Users.Count; i++)
            {
                for (int j = 0; j < data.Tags.Count; j++)
                {
                    if (data.data[j, i] != NULL)
                    {
                        data.data[j, i] = data.data[j, i] - data.data[data.Tags.Count, i];
                    }
                    else
                    {
                        data.data[j, i] = 0;
                    }
                }
            }
            rs = caculateSimilarity(data);
        
            return rs;
        }

        public void caculateAve(AlgorithmData data)
        {
            double ave = 0;
            int count = 0;
            for (int i = 0; i < data.Users.Count; i++)
            {
                ave = 0;
                count = data.Tags.Count;
                for (int j = 0; j < data.Tags.Count; j++)
                {
                    if (data.data[j, i] != NULL)
                    {
                        ave += data.data[j, i];
                    }
                    else
                    {
                        count--;
                    }
                }

                data.data[data.Tags.Count, i] = ave / count;
            }

        }

        public double[,] caculateSimilarity(AlgorithmData data)
        {
            double[] rsTmp1 = new double[data.Tags.Count];
            double[] rsTmp2 = new double[data.Tags.Count];

            double[,] rs = new double[data.Users.Count, data.Users.Count];
            for (int i = 0; i < data.Users.Count; i++)
            {
                rsTmp1 = getCol(i, data);
                for (int j = 0; j < data.Users.Count; j++)
                {
                    rsTmp2 = getCol(j, data);
                    rs[i, j] = 1- cosineSimilarity(rsTmp1, rsTmp2);
                }
            }
            return rs;
        }

        public double cosineSimilarity(double[] vectorA, double[] vectorB)
        {
            double dotProduct = 0.0;
            double normA = 0.0;
            double normB = 0.0;
            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                normA += Math.Pow(vectorA[i], 2);
                normB += Math.Pow(vectorB[i], 2);
            }
            double rs = dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB));
            if (double.IsNaN(rs)) return 0;
            return rs;
        }
        public double[] getCol(int index, AlgorithmData data)
        {
            double[] rs = new double[data.Tags.Count];
            for (int i = 0; i < data.Tags.Count; i++)
            {
                rs[i] = data.data[i, index];
            }
            return rs;
        }
        public double[] getColInMatrix(int index, double[,] data, int size)
        {
            double[] rs = new double[size];
            for (int i = 0; i < size; i++)
            {
                rs[i] = data[index, i];
            }
            return rs;
        }


        public int[] ParserUserToInt(List<Users> users)
        {
            int[] rs = new int[users.Count];

            for (int i = 0; i < users.Count; i++)
            {
                rs[i] = users[i].Id;
            }
            return rs;
        }
        public List<suggestion> MostSimilarity(int limit, AlgorithmData algorithmData, double[,] data)
        {

            if (limit <= 0)
            {
                limit = algorithmData.Users.Count;
            }
            List<suggestion> rs = new List<suggestion>();
            int[] SuggestedUsers;
          
            for (int i = 0; i < algorithmData.Users.Count; i++)
            {
                int[] userparse = ParserUserToInt(algorithmData.Users);
                SuggestedUsers = sortWeight(userparse, getColInMatrix(i, data, algorithmData.Users.Count), limit);
                var test = new int[2];
                Array.Copy(SuggestedUsers, 0, test, 0, 2);
                
                suggestion suggestion = new suggestion(algorithmData.Users[i], test);
                rs.Add(suggestion);
                //            System.out.print(algorithmData.getUsers().get(i) + " || ");
                //            for (int k :suggestedUsers ) {
                //                System.out.print(k+" ");
                //            }

            }
            return rs;
        }

        public int[] sortWeight(int[] users, double[] weight, int limit)
        {
            int[] userTmp = (int[])users.Clone();
            double[] weightTmp = (double[])weight.Clone();
            //if (users.Count != weight.Count)
            //{
            //    throw new ParaException("Size of 2 lists is not eual");
            //}
            if (limit <= 0)
            {
                limit = weight.Length;
            }
            for (int i = 0; i < weightTmp.Length; i++)
            {
                for (int j = 0; j < weightTmp.Length; j++)
                {
                    if (weightTmp[i] < weightTmp[j])
                    {
                        double temp = weightTmp[i];
                        weightTmp[i] = weightTmp[j];
                        weightTmp[j] = temp;
                        int temp2 = userTmp[i];
                        userTmp[i] = userTmp[j];
                        userTmp[j] = temp2;
                    }
                }
            }

            return (int[])userTmp.Clone();
        }

        
    }
}
