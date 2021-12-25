namespace Aviant.DDD.Infrastructure.Transport.Kafka;

public sealed record EventsProducerConfig
{
    public EventsProducerConfig(string kafkaConnectionString, string topicName)
    {
        if (string.IsNullOrWhiteSpace(kafkaConnectionString))
            throw new ArgumentNullException(nameof(kafkaConnectionString));

        if (string.IsNullOrWhiteSpace(topicName))
            throw new ArgumentNullException(nameof(topicName));

        KafkaConnectionString = kafkaConnectionString;
        TopicName         = topicName;
    }

    internal string KafkaConnectionString { get; }

    internal string TopicName         { get; }
}
