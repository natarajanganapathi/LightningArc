namespace LightningArc.Contracts;

public interface ICommandResponse<TEntity> : IApiResponse
{
    TEntity? Data { get; set; }
}
