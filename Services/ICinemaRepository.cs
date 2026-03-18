using Microsoft.AspNetCore.Http;
using EventGo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventGo.Services
{
    public interface ICinemaRepository
    {
        int delete(int id);
        List<Cinema> GetAll();
        Cinema GetById(int id);
        Cinema GetByLocation(string location);
        Cinema GetByName(string name);
       Task<int> insert(Cinema newCinema, List<IFormFile> Image);
        Task<int> update(Cinema EditCin, int id, List<IFormFile> Image);
    }
}