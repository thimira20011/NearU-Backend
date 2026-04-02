using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NearU_Backend_Revised.DTOs.Job;
using NearU_Backend_Revised.Services.Interfaces;
using NearU_Backend_Revised.Models;
using System.Security.Claims;

namespace NearU_Backend_Revised.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobservice;

        public JobController(IJobService jobservice)
        {
            _jobservice = jobservice;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            try
            {
                var jobs = await _jobservice.GetAllJobsAsync();
                return
                Ok(ApiResponse<IEnumberable<JobResponse>>.SuccessResponse("Jobs retrived successfully", jobs));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailResponse(ex.Message));
            }
        }

        [HttpGet("new")]
        public async Task<IActionResult> GetNewJobs()
        {
            try
            {
                var jobs = await _jobservice.GetNewJobsAsync();
                return Ok(ApiResponse<IEnumerable<JobResponse>>.SuccessResponse("New jobs retrived successfully", jobs));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailResponse(ex.Message));
            }

        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetJobsByCategory(string category)
        {
            try
            {
                var jobs = await _jobservice.GetJobsByCategoryAsync(category);
                return Ok(ApiResponse<IEnumerable<JobResponse>>.SuccessResponse($"Jobs in category '{category}' retrived successfully", jobs));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailResponse(ex.Message));
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(string id)
        {
            try
            {
                var job = await _jobservice.GetJobByIdAsync(id);
                if (job == null)
                    return NotFound(ApiResponse<object>.FailResponse("Job not found"));
                return Ok(ApiResponse<JobResponse>.SuccessResponse("Job retrived successfully", job));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailResponse(ex.Message));
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateJob(CreateJob dto)
        {
            try
            {
                var userId = User.FindFirstValue("userId")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return
                        Unauthorized(ApiResponse<object>.FailResponse("User not authenticated"));
                var job = await _jobservice.CreateJobAsync(dto, userId);
                return
                    Created($"/api/job/{job.Id}", ApiResponse<JobResponse>.SuccessResponse("Job created successfully", job));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailResponse(ex.Message));

            }

        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateJob(string id, UpdateJob dto)
        {
            try
            {
                var userId = User.FindFirstValue("userId")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<object>.FailResponse("User not authenticated"));
                var job = await _jobservice.UpdateJobAsync(id, dto, userId);
                if (job == null)
                    return NotFound(ApiResponse<object>.FailResponse("Job not found or user not authorized"));
                return Ok(ApiResponse<JobResponse>.SuccessResponse("Job updated successfully", job));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailResponse(ex.Message));
            }
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteJob(string id)
        {
            try
            {
                var userId = User.FindFirstValue("userId")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<object>.FailResponse("User not authenticated"));
                var success = await _jobservice.DeleteJobAsync(id, userId);
                if (!success)
                    return NotFound(ApiResponse<object>.FailResponse("Job not found or user not authorized"));
                return Ok(ApiResponse<object>.SuccessResponse("Job deleted successfully", null));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailResponse(ex.Message));
            }
        }
    }
}