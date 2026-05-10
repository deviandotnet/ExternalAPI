using ExternalAPI.DTOs;
using ExternalAPI.Helpers;
using ExternalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExternalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly RecordsDbContext _context;
        private readonly RabbitPublisher _publisher;
        public RecordsController(RecordsDbContext context, RabbitPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        //post
        [HttpGet]
        public async Task<ActionResult<RecordResponse>> GetAllRecords()
        {
            var records = await _context.Records
                .AsNoTracking()
                .Select(x => new RecordResponse
                {
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name,

                })
                .ToListAsync();

            return Ok(records);

        }

        //post
        [HttpGet("{id}")]
        public async Task<ActionResult<RecordResponse>> GetRecordById(Guid id)
        {
            var record = await _context.Records.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new RecordResponse
                {
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name,

                }).SingleOrDefaultAsync();

            if (record is null)
            {
                return NotFound();
            }

            return Ok(record);

        }

        [HttpPost]
        public async Task<ActionResult<RecordResponse>> AddRecord(RecordRequest request)
        {
            var newRecord = new Record
            {
                Id = Guid.NewGuid(),
                Description = request.Description,
                Name = request.Name,
                RecordNumber = request.RecordNumber
            };

            await _context.Records.AddAsync(newRecord);
            await _context.SaveChangesAsync();

            var recordEvent = new RecordCreatedEvent
            {
                CorrelationId = Guid.NewGuid(),
                //Version = 1,
                Data = new RecordResponse
                {
                    Description = newRecord.Description,
                    Id = newRecord.Id,
                    Name = newRecord.Name,
                    RecordNumber = newRecord.RecordNumber
                }
            };

            await _publisher.PublishAsync(recordEvent);
                      
            return CreatedAtAction(nameof(GetRecordById), new { newRecord.Id }, new { Message = "Record created successfully" });
        }

    }
}
