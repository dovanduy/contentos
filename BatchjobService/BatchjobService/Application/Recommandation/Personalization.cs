using BatchjobService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Recommandation
{
    public class suggestion
    {
        public Users Reciever { get; set; }
        public int[] Suggestion { get; set; }

        public suggestion(Users reciever, int[] suggestion)
        {
            Reciever = reciever;
            Suggestion = suggestion;
        }
    }
    public class Personalization
    {
        public Personalization()
        {
        }

        public Personalization(int userId, int tagId)
        {
            
            UserId = userId;
            TagId = tagId;
        }



        public int UserId { get; set; }
        public int TagId { get; set; }
    }

    public class TimeInteraction
    {
        public bool IsChosen { get; set; }
        public int UserId { get; set; }
        public int TagId { get; set; }
        public double time { get; set; }
        public bool IsSuggest { get; set; }

    }

    public class AlgorithmData
    {
        public AlgorithmData()
        {
        }

        public AlgorithmData(double[,] data, List<Users> users, List<Tags> tags)
        {
            this.data = data;
            Users = users;
            Tags = tags;
        }

        public List<Users> Users { get; set; }
        public List<Tags> Tags { get; set; }

        public double[,] data { get; set; }
    }
}
