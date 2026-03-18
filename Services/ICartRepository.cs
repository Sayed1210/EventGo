using EventGo.Models;
using EventGo.ViewModels;
using System.Collections.Generic;

namespace EventGo.Services
{
    public interface ICartRepository
    {
        public List<Cart> GetData(Cart cart);
  
        public void Insert(Cart mic);
        public void Delete(Cart cart);
    }
}
