using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Consts;
public static class RateLimiters
{
    public const string IpLimiter = "ipLimit";                // Limits requests from the same IP address to 2 requests every 20 seconds;
    public const string UserLimiter = "userLimit";                // Limits requests from the same user ID to 2 requests every 20 seconds
    public const string Concurrency = "concurrency";      // Limits the number of concurrent requests to 1000 and allows up to 100 requests in the queue

}
