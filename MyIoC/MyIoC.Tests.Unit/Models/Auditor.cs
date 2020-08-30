using Microsoft.Extensions.Logging;
using MyIoC.Tests.Unit.Contracts;

namespace MyIoC.Tests.Unit.Models
{
    internal class Auditor : IAuditor
    {
        private readonly ILogger<Auditor> _logger;

        public Auditor(ILogger<Auditor> logger)
        {
            _logger = logger;
        }
    }
}
