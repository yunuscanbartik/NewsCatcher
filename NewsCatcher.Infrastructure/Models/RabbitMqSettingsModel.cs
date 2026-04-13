namespace NewsCatcher.Infrastructure.Models
{
    /// <summary>RabbitMQ client settings. HostName, UserName, and Password must be set in configuration.</summary>
    public class RabbitMqSettingsModel
    {
        public string HostName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; }

        /// <summary>Recovery interval when automatic recovery is enabled; 0 uses implementation default.</summary>
        public int NetworkRecoveryIntervalSeconds { get; set; }

        /// <summary>Consumer QoS prefetch count; 0 uses implementation default.</summary>
        public int ConsumerPrefetchCount { get; set; }
    }
}
