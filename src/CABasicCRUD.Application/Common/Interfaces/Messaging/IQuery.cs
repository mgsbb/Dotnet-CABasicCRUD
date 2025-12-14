using CABasicCRUD.Domain.Common;
using MediatR;

namespace CABasicCRUD.Application.Common.Interfaces.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
