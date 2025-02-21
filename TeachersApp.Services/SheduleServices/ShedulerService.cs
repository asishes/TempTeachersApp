using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Services.Interfaces;

namespace TeachersApp.Services.SheduleServices
{
    public class ShedulerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ShedulerService> _logger;

        public ShedulerService(IServiceProvider serviceProvider, ILogger<ShedulerService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.UtcNow;
                    var nextRun = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(1); // Run at midnight
                    var delay = nextRun - now;

                    _logger.LogInformation($"Next scheduled run at: {nextRun}, Delay: {delay.TotalMilliseconds}ms");

                    await Task.Delay(delay, stoppingToken); // Wait until the next midnight

                    // Execute the promotion update logic
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var promotionService = scope.ServiceProvider.GetRequiredService<IPromotionService>();
                        var teacherService = scope.ServiceProvider.GetRequiredService<ITeacherService>(); // Assuming this has ProcessRetirementsAsync

                        // Task 1: Process retirements
                        await teacherService.ProcessRetirementsAsync();
                        _logger.LogInformation($"Retirement processing completed at {DateTime.UtcNow}");

                        // Task 1: Update promotion seniority for all employees
                        await promotionService.UpdatePromotionSeniorityAsync();
                        _logger.LogInformation($"Seniority update completed at {DateTime.UtcNow}");
                        // Task 2: Fetch sorted promotion-eligible employees

                        var sortedEmployees = await promotionService.GetSortedPromotionEligibleEmployeesAsync();
                        _logger.LogInformation($"Sorted promotion-eligible employees retrieved at {DateTime.UtcNow}");

                        // You can log or process the sorted employees list as needed
                        _logger.LogInformation($"Total eligible employees: {sortedEmployees.Count}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during scheduled job execution: {ex.Message}");
                }
            }
        }
    }

}
