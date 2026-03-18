using Microsoft.AspNetCore.Http;
using EventGo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventGo.Services
{
    public interface IUpdateProfileRepository
    {
        User GetById(string id);
        Task<int> insert(User NewUser, List<IFormFile> Image);
        Task<int> updateAsync(string id, User UpdateUser, List<IFormFile> Image);
    }
}