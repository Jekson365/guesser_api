using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Interfaces;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        public ApplicationDbContext _context;
        private static Queue<Image> _imageRecords = new Queue<Image>();
        private readonly IImageInterface _imageRepo;
        public ImageController(IImageInterface imageInterface, ApplicationDbContext context)
        {
            _context = context;
            _imageRepo = imageInterface;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _imageRepo.GetAll();
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDraft(UpdateImageDto updateImageDto)
        {
            var currentImage = await _context.Images.FindAsync(updateImageDto.ImageId);

            if (currentImage == null)
            {
                return NotFound();
            }

            currentImage.Drafted = updateImageDto.Drafted;

            await _context.SaveChangesAsync();

            return Ok(new { image = currentImage, updated = true });
        }

        [HttpGet("load")]
        public async Task<IActionResult> LoadImagesQueue()
        {

            var result = await _context.Images.Where(i => i.Drafted == false)
            .Include(i => i.Sender).OrderBy(x => Guid.NewGuid()).Take(5).ToListAsync();

            _imageRecords = new Queue<Image>(result);

            return Ok(new { status = 200, message = "loaded", count = _imageRecords.Count() });
        }

        [HttpGet("next")]
        public async Task<IActionResult> GetNextImage()
        {
            if (_imageRecords.Count == 0)
            {
                return Ok(new { message = "ended", status = 404 });
            }
            var currentImage = _imageRecords.Dequeue();

            return Ok(currentImage);
        }
        [HttpGet("records")]
        public async Task<IActionResult> GameStartedImages()
        {
            var result = await GetRandomImage();
            return Ok(result);
        }
        [HttpPost("answer")]
        public IActionResult GetAnswer([FromBody] CordDto cordDto)
        {
            double distance = CalculateDistance(cordDto.Lat, cordDto.Lng, cordDto.AnsweredLat, cordDto.AnsweredLng);

            var response = new
            {
                distance = distance,
                lat = cordDto.Lat,
                lng = cordDto.Lng,
                questionsRemaining = _imageRecords.Count()
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateImageDto createImageDto)
        {
            var result = await _imageRepo.Create(createImageDto);
            return Ok(result);
        }
        private async Task<Image?> GetRandomImage()
        {
            var totalCount = await _context.Images.CountAsync();
            var random = new Random();
            int randomIndex = random.Next(0, totalCount);

            var record = await _context.Images
            .Skip(randomIndex).Take(1).FirstOrDefaultAsync();

            return record;
        }
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radius of the Earth in km
            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // Distance in km
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

    }
}