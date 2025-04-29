using System;
using System.Threading;
using System.Threading.Tasks;
using GharBhada.Repositories.SpecificRepositories.AgreementRepositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GharBhada.Utils
{
    public class ExpiredAgreementService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExpiredAgreementService> _logger;

        public ExpiredAgreementService(
            IServiceProvider serviceProvider,
            ILogger<ExpiredAgreementService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new(TimeSpan.FromHours(2));

         
            await CheckExpiredAgreements(stoppingToken);

            // Then run every 2 hours
            while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                await CheckExpiredAgreements(stoppingToken);
            }
        }

        private async Task CheckExpiredAgreements(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Checking for expired agreements at: {time}", DateTimeOffset.Now);

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var agreementRepositories =
                        scope.ServiceProvider.GetRequiredService<IAgreementRepositories>();

                    await agreementRepositories.UpdatePropertyStatusForAllExpiredAgreementsAsync();
                }

                _logger.LogInformation("Successfully updated expired agreements and property statuses");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking expired agreements");
            }
        }
    }
}