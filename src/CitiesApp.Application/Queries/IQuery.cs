using MediatR;

namespace CitiesApp.Application.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
