using AutoMapper;
using DoorAccessApplication.Api.Models;
using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoorAccessApplication.Api.Controllers
{
    [Route("api/locks")]
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

            var addedlock = await _lockService.CreateAsync(lockTool, GetUserId());

            var lockResponse = _mapper.Map<LockResponse>(addedlock);
            _logger.LogInformation($"Lock with Id: {lockResponse.Id} added.");

            return Ok(lockResponse);
        }

        [HttpGet]
        public async Task<ActionResult<List<Lock>>> GetAllAsync()
        {
            var lockTools = await _lockService.GetAllAsync(GetUserId());
            var lockResponse = _mapper.Map<List<LockResponse>>(lockTools);

            return Ok(lockResponse);
        }

        [HttpGet("{lockId}")]
        public async Task<ActionResult<Lock>> GetAsync(int lockId)
        {
            var lockTools = await _lockService.GetAsync(lockId, GetUserId());
            var locksResponse = _mapper.Map<LockWithUsersResponse>(lockTools);

            return Ok(locksResponse);
        }

        [HttpDelete("{lockId}")]
        public async Task<IActionResult> RemoveAsync(int lockId)
        {
            await _lockService.DeleteAsync(lockId, GetUserId());

            _logger.LogInformation($"Lock with Id: {lockId} deleted");

            return Ok();
        }

        [HttpPut("{lockId}/users/{email}")]
        public async Task<IActionResult> AddUserAsync(int lockId, string email)
        {
            var lockTool = await _lockService.AddUserAsync(lockId, GetUserId(), email);

            var lockResponse = _mapper.Map<LockWithUsersResponse>(lockTool);

            _logger.LogInformation($"User was added to lock with Id: {lockResponse.Id}.");

            return Ok(lockResponse);
        }

        [HttpDelete("{lockId}/users/{email}")]

        public async Task<IActionResult> RemoveUserAsync(int lockId, string email)
        {
            var lockTool = await _lockService.RemoveUserAsync(lockId, GetUserId(), email);

            var lockResponse = _mapper.Map<LockWithUsersResponse>(lockTool);

            _logger.LogInformation($"User was added to lock with Id: {lockResponse.Id}.");

            return Ok(lockResponse);
        }

        [HttpPut("{lockId}/statuses/{status}")]
        public async Task<IActionResult> UpdateStatusAsync(int lockId, string status)
        {
            var lockTool = await _lockService.UpdateStatusAsync(lockId, GetUserId(), status);

            var lockResponse = _mapper.Map<LockResponse>(lockTool);

            _logger.LogInformation($"User updated status to {status} of a lock with Id: {lockId}");
            
            return Ok(lockResponse);
        }

        [HttpGet("{lockId}/history")]
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
