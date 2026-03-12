using MediatR;
using LostAndFound.Application.DTOs.Item;

namespace LostAndFound.Application.Features.Items.Queries.GetItems;

public class GetItemsQuery : IRequest<IEnumerable<ItemResponseDto>>
{
    // Aquí se podrían añadir parámetros de búsqueda o paginación en el futuro
}
