namespace Lab5.Application.Models.Operations;

public record Operation(long OperationId, long UserId, string Type, long Amount, string Time);