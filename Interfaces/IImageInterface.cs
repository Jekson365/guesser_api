using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dto;
using server.Models;

namespace server.Interfaces
{
    public interface IImageInterface
    {
        Task<List<Image>> GetAll();
        Task<CreateImageDto> Create(CreateImageDto createImageDto);
    }
}