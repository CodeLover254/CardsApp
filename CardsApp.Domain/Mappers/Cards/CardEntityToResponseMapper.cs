using CardsApp.Domain.Dto.Cards;
using CardsApp.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace CardsApp.Domain.Mappers.Cards;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public partial class CardEntityToResponseMapper
{
    public partial CardResponse MapToResponse(Card card);
    public partial IEnumerable<CardResponse> MapToResponseList(IEnumerable<Card> cards);
}