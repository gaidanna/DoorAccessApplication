using AutoMapper;
using DoorAccessApplication.Api.Models;
using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<IActionResult> AddAsync(CreateLockRequest createLockRequest)
        {
            var lockTool = _mapper.Map<Lock>(createLockRequest);

            var lockResult = await _lockService.AddAsync(lockTool, GetUserId());

            _logger.LogInformation($"Lock with Id: {lockResult.Id} added.");

            return Ok(lockResult);
        }

        [HttpDelete("{lockId}")]
        public async Task<IActionResult> RemoveAsync(int lockId)
        {
            await _lockService.DeleteAsync(lockId, GetUserId());

            _logger.LogInformation($"Lock with Id: {lockId} deleted");

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<Lock>>> GetAllAsync()
        {
            var lockTools = await _lockService.GetAllAsync(GetUserId());

            return Ok(lockTools);
        }


        [HttpGet("{lockId}")]
        public async Task<ActionResult<Lock>> GetAsync(int lockId)
        {
            var lockTool = await _lockService.GetAsync(lockId, GetUserId());

            return Ok(lockTool);
        }

        //incorrect path, update
        [HttpPut("users/{lockId}")]
        public async Task<IActionResult> AddUserAsync(int lockId, AddUserInLockRequest addRequest)
        {
            var lockResult = await _lockService.AddUserAsync(lockId, GetUserId(), addRequest.Email);

            _logger.LogInformation($"User was added to lock with Id: {lockResult.Id}.");

            return Ok(lockResult);
        }

        [HttpPut("{lockId}")]
        public async Task<IActionResult> RemoveUserAsync(int lockId, RemoveUserInLockRequest removeRequest)
        {
            var lockResult = await _lockService.RemoveUserAsync(lockId, GetUserId(), removeRequest.Email);

            _logger.LogInformation($"User was added to lock with Id: {lockResult.Id}.");

            return Ok(lockResult);
        }

        [HttpPut("actions/{lockId}")]
        public async Task<IActionResult> UpdateActionAsync(int lockId)
        {
            var lockResult = await _lockService.UpdateStatusAsync(lockId, GetUserId());
            if(lockResult.IsLocked)
            {
                _logger.LogInformation($"User closed the lock with Id: {lockId}.");
            }
            else
            {
                _logger.LogInformation($"User opened the lock with Id: {lockId}.");
            }

            return Ok(lockResult);
        }

        [HttpGet("history/{lockId}")]
        public async Task<ActionResult<LockHistoryEntry>> ShowHistoryAsync(int lockId)
        {
            var lockHistoryEntries = await _lockService.GetHistoryAsync(lockId, GetUserId());

            return Ok(lockHistoryEntries);
        }


        private string GetUserId()
        {
            //return "0271ac86-7ba2-40f4-ac6d-cb3bc173b3aa";
            return User.Claims.First(i => i.Type == "UserId").Value;
        }
    }
}
