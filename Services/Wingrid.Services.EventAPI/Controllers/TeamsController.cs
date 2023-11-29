using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wingrid.Services.EventAPI.Models.Dto;
using Wingrid.Services.EventAPI.Services;

namespace Wingrid.Services.EventAPI.Controllers
{
    [Route("api/teams")]
    [ApiController]
    // [Authorize]
    public class TeamsController(ITeamsService teamsService, IMapper mapper) : BaseController<TeamsController>
    {
        private readonly ITeamsService _teamsService = teamsService;
        private readonly IMapper _mapper = mapper;

    [HttpGet]
    [Route("{id}")]
    public async Task<ResponseDto> Get(string id)
    {
       return await ExecuteActionAsync(async () => {
            var team = await _teamsService.GetTeamAsync(id);
            return _mapper.Map<TeamDto>(team);
       });
    }
    }
}