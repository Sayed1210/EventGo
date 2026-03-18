using EventGo.Models;
using System.Collections.Generic;

namespace EventGo.ViewModels
{
    public class MovieDetailsViewModel
    {
        public virtual Movie Movie { get; set; }
        public virtual string UserId { get; set; }
        public virtual List<Cart> carts { get; set; }
        public virtual List<MovieActor> MovieActors { get; set; }
        public virtual List<MovieInCinema> MoviesInCinemas { get; set; }

    }
}
