namespace Aviant.DDD.Infrastructure.Persistence.Kafka;

public sealed class EventConsumerConfig
{
    public EventConsumerConfig(
        string kafkaConnectionString,
        string topicBaseName,
        string consumerGroup)
    {
        if (string.IsNullOrWhiteSpace(kafkaConnectionString))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(kafkaConnectionString));

        if (string.IsNullOrWhiteSpace(topicBaseName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(topicBaseName));

        if (string.IsNullOrWhiteSpace(consumerGroup))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(consumerGroup));

        KafkaConnectionString = kafkaConnectionString;
        TopicBaseName         = topicBaseName;
        ConsumerGroup         = consumerGroup;
    }

    internal string KafkaConnectionString { get; }

    internal string TopicBaseName { get; }

    internal string ConsumerGroup { get; }
}
