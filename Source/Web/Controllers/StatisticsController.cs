using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Services;
using Wingrid.Controllers;
using Wingrid.Models.Dto;

namespace Web.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController(IStatisticsService statisticsService, IMapper mapper) : BaseController<TeamsController>
    {
        private readonly IStatisticsService _statisticsService = statisticsService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Authorize]
        public async Task<ResponseDto> Get()
        {
            return await ExecuteActionAsync(async () =>
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Missing or invalid user id.");
                var fixtures = await _statisticsService.GetStatistics();
                return _mapper.Map<IEnumerable<FixtureDto>>(fixtures);
            });
        }

    }
}