using System;

namespace DepositStopLoss.Domain.Deposits;

/// <summary>
///     Strongly typed identity for DepositContribution entity.
/// </summary>
public readonly record struct DepositContributionIdentity(Guid Value)
{
    public static DepositContributionIdentity Empty => new(Guid.Empty);

    public static DepositContributionIdentity New()
    {
        return new DepositContributionIdentity(Guid.NewGuid());
    }
}
