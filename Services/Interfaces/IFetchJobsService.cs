using Bot.DTO;
using Bot.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Services.Interfaces
{
    public interface IFetchJobsService
    {
        Task<IEnumerable<InsideData>> FetchAsync(JobSearchCriteria jobSearchCriteria);
    }
}
