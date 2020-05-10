using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace AuthenticationService.Application.Queries.GetAds
{


    public class GetAdsRequest : IRequest<List<ContentViewer>>
    {
    }
}
