using AutoMapper;
using DoorAccessApplication.Api.Models;
using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoorAccessApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> AddLockAsync(CreateLockRequest createLockRequest)
        {
            var lockTool = _mapper.Map<Lock>(createLockRequest);

            var lockResult = await _lockService.CreateAsync(lockTool);

            _logger.LogInformation("Lock added.");

            return Ok(lockResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLockAsync(int id, UpdateLockRequest updateLockRequest)
        {
            if (id != updateLockRequest.Id)
            {
                return BadRequest("Prohibited to change Id.");
            }

            var lockTool = _mapper.Map<Lock>(updateLockRequest);
            var result = await _lockService.UpdateAsync(lockTool);

            _logger.LogInformation($"Lock with Id: {id} updated");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveLockAsync(int id)
        {
            await _lockService.DeleteAsync(id);

            _logger.LogInformation($"Lock with Id: {id} deleted");

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<Lock>>> GetLocksAsync()
        {
            var lockTools = await _lockService.GetAllAsync();

            return Ok(lockTools);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Lock>> GetLockByIdAsync(int id)
        {
            var lockTool = await _lockService.GetAsync(id);

            return Ok(lockTool);
        }
    }
}
