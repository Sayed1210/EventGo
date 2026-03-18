using EventGo.Models;
using EventGo.ViewModels;

namespace EventGo.Services
{

    public class MovieOrderRepository : IMovieOrderRepository
    {
        MovieContext db;
        public MovieOrderRepository(MovieContext _db)
        {
            db = _db;
        }
        public void insert(MovieOrder order)
        {
            db.MovieOrders.Add(order);
            db.SaveChanges();
        }
    }
}
