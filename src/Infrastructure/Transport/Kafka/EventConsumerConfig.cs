namespace Aviant.DDD.Infrastructure.Transport.Kafka;

public sealed class EventConsumerConfig
{
    public EventConsumerConfig(
        string kafkaConnectionString,
        string topicBaseName,
        string consumerGroup)
    {
        if (string.IsNullOrWhiteSpace(kafkaConnectionString))
            throw new ArgumentNullException(nameof(kafkaConnectionString));

        if (string.IsNullOrWhiteSpace(topicBaseName))
            throw new ArgumentNullException(nameof(topicBaseName));

        if (string.IsNullOrWhiteSpace(consumerGroup))
            throw new ArgumentNullException(nameof(consumerGroup));

        KafkaConnectionString = kafkaConnectionString;
        TopicBaseName         = topicBaseName;
        ConsumerGroup         = consumerGroup;
    }

    internal string KafkaConnectionString { get; }

    internal string TopicBaseName { get; }

    internal string ConsumerGroup { get; }
}
