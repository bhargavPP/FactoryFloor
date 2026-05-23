using FactoryFloor.Contracts;
using MassTransit;

namespace FactoryFloor.NotificationService.Consumers
{
    public class MachineCreatedConsumer : IConsumer<MachineCreatedEvent>
    {
        private readonly ILogger<MachineCreatedConsumer> _logger;
        public MachineCreatedConsumer(ILogger<MachineCreatedConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<MachineCreatedEvent> context)
        {
            var evt = context.Message;

            _logger.LogInformation("Received MachineCreatedEvent: MachineId={MachineId}, TenantId={TenantId}, MachineName={MachineName}, SerialNumber={SerialNumber}, Location={Location}, CreatedAt={CreatedAt}",
                evt.MachineId, evt.TenantId, evt.MachineName, evt.SerialNumber, evt.Location, evt.CreatedAt);

            await Task.CompletedTask;
        }
    }
}
