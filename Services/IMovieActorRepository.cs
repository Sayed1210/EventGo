using EventGo.Models;
using System.Collections.Generic;

namespace EventGo.Services
{
    public interface IMovieActorRepository
    {
        public List<MovieActor> GetAll();
    }
}
