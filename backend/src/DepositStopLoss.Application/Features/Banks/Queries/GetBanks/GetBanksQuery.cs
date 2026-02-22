using System.Collections.Generic;

using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

using FastEndpoints;

namespace DepositStopLoss.Application.Features.Banks.Queries.GetBanks;

/// <summary>
///     Query to get all active banks.
/// </summary>
public sealed record GetBanksQuery(bool OnlyActive = true) : ICommand<ErrorOr<IReadOnlyList<BankDto>>>;
