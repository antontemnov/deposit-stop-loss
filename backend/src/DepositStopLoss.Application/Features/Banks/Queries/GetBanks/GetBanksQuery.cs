using System.Collections.Generic;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

namespace DepositStopLoss.Application.Features.Banks.Queries.GetBanks;

/// <summary>
///     Query to get all active banks.
/// </summary>
public sealed record GetBanksQuery(bool OnlyActive = true) : IQuery<ErrorOr<IReadOnlyList<BankDto>>>;
