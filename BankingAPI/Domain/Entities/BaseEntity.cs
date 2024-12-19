﻿namespace BankingAPI.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; protected internal set; }
    public bool IsDeleted { get; set; }
}