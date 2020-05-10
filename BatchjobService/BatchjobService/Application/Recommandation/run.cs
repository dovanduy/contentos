using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Recommandation
{
    public interface IRun
    {
        Task<bool> Handle();
    }
    public class Run : IRun
    {
        private readonly IAlgorithmDataLogic _Logic;
        private readonly IAlgorithmn _Algorithm;

        public Run(IAlgorithmDataLogic Logic, IAlgorithmn Algorithm)
        {
            _Logic = Logic;
            _Algorithm = Algorithm;
        }
        public async Task<bool> Handle()
        { 
            var Data = new  AlgorithmData();
            var AlData = new ModelAlgorithm(); 
            try
            {
                 _Logic.UpdateSuggestion();
                var dataUpdate = await _Logic.AlgorithmDataBefore();
                _Logic.UpdateTimeInteraction(dataUpdate);
                AlData = await _Logic.GetDataAsync();
                Data.data = AlData.data;
                Data.Users = AlData.users;
                Data.Tags = AlData.tags;

                double[,] rs = _Algorithm.run(Data);
                List<suggestion> _suggest = _Algorithm.MostSimilarity(2, Data, rs);
                //AlgorithmDataLogic updateDb = new AlgorithmDataLogic();
                for (int i = 0; i < _suggest.Count ; i++)
                {
                    //int[] Rlist = _suggest[i].Suggestion;
                    if (_suggest[i].Suggestion.Length > 1)
                    {
                        for (int j = 1; j < _suggest[i].Suggestion.Length; j++)
                        {
                            await _Logic.CreateSuggestionAsync(_suggest[i].Reciever.Id, _suggest[i].Suggestion[j]);

                        }
                    }
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
            
        }

       
    }
}
