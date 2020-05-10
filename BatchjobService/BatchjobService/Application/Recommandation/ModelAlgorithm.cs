using BatchjobService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Recommandation
{
    public class ModelAlgorithm
    {
        public double[,] data;
        public List<Users> users;
        public List<Tags> tags;
    }
}
