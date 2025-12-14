using CABasicCRUD.Domain.Common;
using MediatR;

namespace CABasicCRUD.Application.Common.Interfaces.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
