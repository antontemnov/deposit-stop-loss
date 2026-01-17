using System;

namespace DepositStopLoss.Domain.Snapshots;

/// <summary>
///     Strongly typed ID for DepositSnapshot entity (historical data points).
///     Prevents accidental ID mix-ups at compile time.
/// </summary>
public readonly record struct DepositSnapshotIdentity(Guid Value)
{
    public static DepositSnapshotIdentity Empty => new(Guid.Empty);

    public static DepositSnapshotIdentity New()
    {
        return new DepositSnapshotIdentity(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
