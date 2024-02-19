using Microsoft.AspNetCore.Mvc;
using AutomapperWebAPI.Services;
using AutomapperWebAPI.DTO;
using AutoMapper;
namespace AutomapperWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<List<UserReadDto>> Get()
        {
            var usersFromRepository = _userRepository.GetAllUser();
            var usersReadDto = _mapper.Map<List<UserReadDto>>(usersFromRepository);
            return Ok(usersReadDto);
        }
       
    }
}
