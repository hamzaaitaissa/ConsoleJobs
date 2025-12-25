using Bot.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Services.Interfaces
{
    public interface IUserInputService
    {
        JobSearchCriteria GetSearchCriteria();
    }
}
