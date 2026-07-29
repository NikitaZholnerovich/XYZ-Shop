using XYZ_shop.Application.Abstractions.Mapping;
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Dtos;

namespace XYZ_shop.Application.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameMapper _gameMapper;

        public CatalogService(IGameRepository gameRepository, IGameMapper gameMapper)
        {
            _gameRepository = gameRepository;
            _gameMapper = gameMapper;
        }

        public HomeCatalogDto GetGamesForHomePage()
        {
            return new HomeCatalogDto
            {
                Featured = _gameRepository.GetFeaturedForHomePage()
                    .Select(_gameMapper.ToDto)
                    .ToList(),
                SpecialOffers = _gameRepository.GetSpecialOffersForHomePage()
                    .Select(_gameMapper.ToDto)
                    .ToList()
            };
        }
    }
}
