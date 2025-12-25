using Bot.Models;
using Bot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Services
{
    public class UserInputService : IUserInputService
    {
        public JobSearchCriteria GetSearchCriteria()
        {
            Console.WriteLine(new string('=', 80));
            Console.WriteLine("====> CONSOLE-JOBS :D");
            Console.WriteLine(new string('=', 80));

            Console.Write("\n====> Enter job keywords (e.g., 'C# developer', 'Python engineer') [default: developer]: ");
            var keywords = Console.ReadLine() ?? "developer";

            Console.Write("====> Enter country code (e.g., 'de', 'us', 'uk') [default: de]: ");
            var country = Console.ReadLine() ?? "de";

            Console.Write("====> Filter by date posted (today/week/month/any) [default: any]: ");
            var datePosted = Console.ReadLine() ?? "any";

            Console.WriteLine();

            return new JobSearchCriteria
            {
                Keywords = keywords,
                Country = country,
                DatePosted = datePosted
            };
        }
    }
}
