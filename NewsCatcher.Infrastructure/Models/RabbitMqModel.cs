namespace NewsCatcher.Infrastructure.Models
{
    public class RabbitMqModel
    {
        public class Publish
        {
            public class Request
            {
                public string Message { get; set; } = string.Empty;
                public string QueueName { get; set; } = string.Empty;
            }

            public class ReturnData
            {
                public bool IsSuccess { get; set; }
                public string? QueueName { get; set; }
                public string? Message { get; set; }
            }
        }

        public class Consume
        {
            public class Request
            {
                public string QueueName { get; set; } = string.Empty;
                public Func<string, Task>? MessageHandler { get; set; }
            }

            public class ReturnData
            {
                public bool IsSuccess { get; set; }
                public string? QueueName { get; set; }
                public string? Message { get; set; }
            }
        }
    }
}



