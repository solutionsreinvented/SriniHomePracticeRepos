using System;

using Microsoft.Extensions.Logging;

namespace DevDrive.LogWorks
{
    public class ExampleService
    {
        private readonly ILogger<ExampleService> _logger;

        public ExampleService(ILogger<ExampleService> logger)
        {
            _logger = logger;
        }

        public void ProcessData(bool shouldSkip)
        {
            _logger.LogInformation("Process data function started.");

            if (shouldSkip)
            {
                _logger.LogWarning("Process data excecution skipped as per condition.");
                return;
            }

            try
            {
                ExecuteCriticalOperation();
                _logger.LogInformation("Process data executed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while executing process data.");
                throw;
            }
        }

        private void ExecuteCriticalOperation()
        {
            throw new InvalidOperationException("Simulated exception.");
        }
    }
}
