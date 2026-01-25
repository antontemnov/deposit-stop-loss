using System;

using NodaTime;

namespace DepositStopLoss.Application.Common.DTOs;

/// <summary>
///     DTO for ExchangeRate entity.
/// </summary>
public sealed record ExchangeRateDto
{
    public required Guid BankId { get; init; }

    public required decimal BuyRate { get; init; }

    public required Instant FetchedAt { get; init; }

    public required string FromCurrency { get; init; }

    public required Guid Id { get; init; }

    public required LocalDate RateDate { get; init; }

    public required string RateType { get; init; }

    public required decimal SellRate { get; init; }

    public required string ToCurrency { get; init; }
}
