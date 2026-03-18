using EventGo.Models;
using EventGo.ViewModels;

namespace EventGo.Services
{
    public interface IMovieOrderRepository
    {

        public void insert(MovieOrder order);
    }
}
