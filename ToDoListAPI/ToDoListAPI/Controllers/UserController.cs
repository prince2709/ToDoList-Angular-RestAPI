using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Contracts.Reposotories;
using ToDoListAPI.DomainModels;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUser _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUser userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var userList = await _userRepository.GetUsersAsync();
            return Ok(_mapper.Map<List<UserDomainModel>>(userList));
        }
    }
}
