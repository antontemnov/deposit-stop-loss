namespace DepositStopLoss.Application;

/// <summary>
///     Marker interface for CQRS commands.
/// </summary>
/// <typeparam name="TResponse">Response type.</typeparam>
public interface ICommand<TResponse>;
