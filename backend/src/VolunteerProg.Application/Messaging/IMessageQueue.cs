namespace VolunteerProg.Application.Messaging;

public interface IMessageQueue<TMessage>
{
    Task WriteAsync(TMessage messages, CancellationToken cancellationToken = default);

    Task<TMessage> ReadAsync(CancellationToken cancellationToken = default);
}