namespace Aviant.Infrastructure.Transport.Kafka;

public sealed record EventsConsumerConfig
{
    public EventsConsumerConfig(
        string kafkaConnectionString,
        string topicName,
        string consumerGroup)
    {
        if (string.IsNullOrWhiteSpace(kafkaConnectionString))
            throw new ArgumentNullException(nameof(kafkaConnectionString));

        if (string.IsNullOrWhiteSpace(topicName))
            throw new ArgumentNullException(nameof(topicName));

        if (string.IsNullOrWhiteSpace(consumerGroup))
            throw new ArgumentNullException(nameof(consumerGroup));

        KafkaConnectionString = kafkaConnectionString;
        TopicName         = topicName;
        ConsumerGroup         = consumerGroup;
    }

    internal string KafkaConnectionString { get; }

    internal string TopicName { get; }

    internal string ConsumerGroup { get; }
}
