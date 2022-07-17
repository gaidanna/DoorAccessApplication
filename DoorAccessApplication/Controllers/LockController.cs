using AutoMapper;
using DoorAccessApplication.Api.Models;
using DoorAccessApplication.Api.ValueTypes;
using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using DoorAccessApplication.Core.ValueTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DoorAccessApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LockController : ControllerBase
    {
        private readonly ILogger<LockController> _logger;
        private readonly ILockService _lockService;
        private readonly IMapper _mapper;
        public LockController(ILogger<LockController> logger,
            ILockService lockService,
            IMapper mapper)
        {
            _logger = logger;
            _lockService = lockService;
            _mapper = mapper;

        }

        [HttpPost(Name = "Add lock")]
        public async Task<IActionResult> AddAsync(CreateLockRequest createLockRequest)
        {
            var lockTool = _mapper.Map<Lock>(createLockRequest);

            var addedlock = await _lockService.AddAsync(lockTool, GetUserId());

            var lockResponse = _mapper.Map<LockResponse>(addedlock);
            _logger.LogInformation($"Lock with Id: {lockResponse.Id} added.");

            return Ok(lockResponse);
        }

        [HttpDelete("{lockId}", Name = "Remove lock")]
        public async Task<IActionResult> RemoveAsync(int lockId)
        {
            await _lockService.DeleteAsync(lockId, GetUserId());

            _logger.LogInformation($"Lock with Id: {lockId} deleted");

            return Ok();
        }

        [HttpGet(Name = "Get all locks")]
        public async Task<ActionResult<List<Lock>>> GetAllAsync()
        {
            var lockTools = await _lockService.GetAllAsync(GetUserId());
            var lockResponse = _mapper.Map<List<LockResponse>>(lockTools);

            return Ok(lockResponse);
        }


        [HttpGet("{lockId}", Name = "Get lock")]
        public async Task<ActionResult<Lock>> GetAsync(int lockId)
        {
            var lockTools = await _lockService.GetAsync(lockId, GetUserId());
            var locksResponse = _mapper.Map<LockWithUsersResponse>(lockTools);

            return Ok(locksResponse);
        }

        //incorrect path, update
        [HttpPut("users/{lockId}", Name = "Add user to lock")]
        public async Task<IActionResult> AddUserAsync(int lockId, AddUserInLockRequest addRequest)
        {
            var lockTool = await _lockService.AddUserAsync(lockId, GetUserId(), addRequest.Email);

            var lockResponse = _mapper.Map<LockWithUsersResponse>(lockTool);

            _logger.LogInformation($"User was added to lock with Id: {lockResponse.Id}.");

            return Ok(lockResponse);
        }

        [HttpPut("{lockId}", Name = "Remove user to lock")]
        public async Task<IActionResult> RemoveUserAsync(int lockId, RemoveUserInLockRequest removeRequest)
        {
            var lockTool = await _lockService.RemoveUserAsync(lockId, GetUserId(), removeRequest.Email);

            var lockResponse = _mapper.Map<LockWithUsersResponse>(lockTool);

            _logger.LogInformation($"User was added to lock with Id: {lockResponse.Id}.");

            return Ok(lockResponse);
        }

        [HttpPut("actions/{lockId}", Name = "Change lock status")]
        public async Task<IActionResult> UpdateStatusAsync(int lockId, string status)
        {
            //var updatedStatusType = _mapper.Map<StatusType>(statusType);
            var lockTool = await _lockService.UpdateStatusAsync(lockId, GetUserId(), status);

            var lockResponse = _mapper.Map<LockResponse>(lockTool);

            if(lockResponse.IsLocked)
            {
                _logger.LogInformation($"User closed the lock with Id: {lockId}.");
            }
            else
            {
                _logger.LogInformation($"User opened the lock with Id: {lockId}.");
            }

            return Ok(lockResponse);
        }

        [HttpPut("history/{lockId}", Name = "Get lock history")]
        public async Task<ActionResult<List<LockHistoryEntryResponse>>> GetHistoryAsync(int lockId)
        {
            var lockHistoryEntries = await _lockService.GetHistoryAsync(lockId, GetUserId());

            var lockHistoryEntriesResponse = _mapper.Map<List<LockHistoryEntryResponse>>(lockHistoryEntries);

            return Ok(lockHistoryEntriesResponse);
        }

        private string GetUserId()
        {
            return User.Claims.First(i => i.Type == "UserId").Value;
        }
    }
}
