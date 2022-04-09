using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Contracts.DataModels;
using ToDoList.Contracts.Reposotories;
using ToDoListAPI.DomainModels;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTaskController : ControllerBase
    {
        private readonly ITodoTask _todoTaskRepository;
        private readonly IUser _userRepository;
        private readonly IMapper _mapper;
        const string finalStatus = "Completed";
        public ToDoTaskController(ITodoTask todoTaskRepository, IUser userRepository, IMapper mapper)
        {
            _todoTaskRepository = todoTaskRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllTasksAsync()
        {
            var taskList = await _todoTaskRepository.GetTasksAsync();
            return Ok(_mapper.Map<List<ToDoTaskDomainModel>>(taskList));
        }

        [HttpGet]
        [Route("GetTasksByUserID/{userID:int}")]
        public async Task<IActionResult> GetTasksByUserIDAsync([FromRoute] int userID)
        {
            var taskList = await _todoTaskRepository.GetTasksByUserAsync(userID);
            if (taskList == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<List<ToDoTaskDomainModel>>(taskList));
        }

        [HttpGet]
        [Route("{taskID:int}")]
        public async Task<IActionResult> GetTaskByTaskIDAsync([FromRoute] int taskID)
        {
            var taskList = await _todoTaskRepository.GetTaskByTaskIDAsync(taskID);
            if (taskList == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ToDoTaskDomainModel>(taskList));
        }

        [HttpGet]
        [Route("GetAllStatus")]
        public async Task<IActionResult> GetAllStatus()
        {
            var statusList = await _todoTaskRepository.GetAllStatus();
            return Ok(statusList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddNewTaskAsync([FromBody] ToDoTaskDomainModel task)
        {
            if (task.AssignedToUserID.HasValue && task.AssignedToUserID != 0)
            {
                var isValidAssignedTo = await _userRepository.IsUserExistsAsync(task.AssignedToUserID.Value);
                if (isValidAssignedTo == false)
                {
                    ModelState.AddModelError("Assigned To", "User selected for Assigned To is not a valid user");
                }
            }
            if (ModelState.IsValid)
            {
                task.CompletedDate = null;
                var addedTask = await _todoTaskRepository.AddNewTaskAsync(_mapper.Map<ToDoTask>(task));
                if (addedTask == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<ToDoTaskDomainModel>(addedTask));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{taskID:int}")]
        public async Task<IActionResult> UpdateTaskAsync([FromRoute] int taskID, [FromBody] ToDoTaskDomainModel task)
        {
            if (await _todoTaskRepository.IsTaskExistsAsync(taskID))
            {
                if (task.AssignedToUserID.HasValue && task.AssignedToUserID != 0)
                {
                    var isValidAssignedTo = await _userRepository.IsUserExistsAsync(task.AssignedToUserID.Value);
                    if (isValidAssignedTo == false)
                    {
                        ModelState.AddModelError("Assigned To", "User selected for Assigned To is not a valid user");
                    }
                }
                if (task.Status == finalStatus)
                {
                    task.CompletedDate = DateTime.Now.ToString();
                }
                else
                {
                    task.CompletedDate = null;
                }
                if (ModelState.IsValid)
                {
                    var updatedTask = await _todoTaskRepository.UpdateTaskDetailsAsync(taskID, _mapper.Map<ToDoTask>(task));
                    if (updatedTask == null)
                    {
                        return NotFound();
                    }
                    return Ok(_mapper.Map<ToDoTaskDomainModel>(updatedTask));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{taskID:int}")]
        public async Task<IActionResult> DeleteTaskAsync([FromRoute] int taskID)
        {
            if (await _todoTaskRepository.IsTaskExistsAsync(taskID))
            {
                var deletedTask = await _todoTaskRepository.DeleteTaskAsync(taskID);
                return Ok(_mapper.Map<ToDoTaskDomainModel>(deletedTask));
            }
            else
            {
                return NotFound();
            }
        }
    }
}
