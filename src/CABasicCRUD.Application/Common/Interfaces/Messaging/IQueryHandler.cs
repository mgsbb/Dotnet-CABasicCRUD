using CABasicCRUD.Domain.Common;
using MediatR;

namespace CABasicCRUD.Application.Common.Interfaces.Messaging;

public interface IQueryHander<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
