using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Interfaces;
using server.Models;

namespace server.Repositories
{
    public class ImageRepository : IImageInterface
    {
        private readonly ApplicationDbContext _context;
        public ImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateImageDto> Create(CreateImageDto createImageDto)
        {
            var targetDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + createImageDto.Image?.FileName;
            var filePath = Path.Combine(targetDirectory, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                if (createImageDto.Image != null)
                {
                    await createImageDto.Image.CopyToAsync(stream);
                }

                var newImageRecord = new Image
                {
                    Path = uniqueFileName,
                    TookBy = createImageDto.TookBy,
                    Lat = createImageDto.Lat,
                    Long = createImageDto.Long,
                    
                    Sender = new Sender
                    {
                        Name = createImageDto.Sender.Name,
                        Surname = createImageDto.Sender.Surname,
                        InstagramUrl = createImageDto.Sender.InstagramUrl
                    }
                };
                await _context.Images.AddAsync(newImageRecord);
                await _context.SaveChangesAsync();

            }
            return createImageDto;
        }

        public async Task<List<Image>> GetAll()
        {
            var images = await _context.Images.Include(s => s.Sender).ToListAsync();

            return images;
        }
    }
}