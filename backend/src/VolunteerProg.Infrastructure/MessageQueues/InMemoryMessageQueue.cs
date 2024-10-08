using System.Threading.Channels;
using VolunteerProg.Application.FileProvider;
using VolunteerProg.Application.Messaging;

namespace VolunteerProg.Infrastructure.MessageQueues;

public class InMemoryMessageQueue<TMessage> : IMessageQueue<TMessage>
{
    private readonly Channel<TMessage> _channel = Channel.CreateUnbounded<TMessage>();

    public async Task WriteAsync(TMessage messages, CancellationToken cancellationToken = default)
    {
        await _channel.Writer.WriteAsync(messages, cancellationToken);
    }

    public async Task<TMessage> ReadAsync(CancellationToken cancellationToken = default)
    {
         return await _channel.Reader.ReadAsync(cancellationToken);
    }
}