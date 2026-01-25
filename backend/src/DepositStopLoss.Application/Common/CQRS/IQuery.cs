namespace DepositStopLoss.Application;

/// <summary>
///     Marker interface for CQRS queries.
/// </summary>
/// <typeparam name="TResponse">Response type.</typeparam>
public interface IQuery<TResponse>;
