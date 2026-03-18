using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EventGo.Models
{
    public class DbPadel
    {

        public static async Task SeedDB(IApplicationBuilder applicationBuilder)
        {

            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<PadelContext>();

                await db.Database.EnsureCreatedAsync();




                //Seeding Categories
                //Seeding Categories
                //Seeding Categories
                if (!db.Places.Any())
                {
                    db.Places.AddRange(new List<Place>()
                    {
                        new Place() { PlaceName = "Ro Padel", Address="Smouha",ImageName=("Ro Padel.png"),Capacity=4,Price=220 ,Description="Smouha",ImagePath=Path.Combine("images","Ro Padel.png")},
                        new Place() { PlaceName = "Padel Arena", Address="Ibrahimia",ImageName=("Padel Arena.png"),Capacity=6,Price=190,Description="Smouha",ImagePath=Path.Combine("images","Padel Arena.png")},
                        new Place() { PlaceName = "Padel West", Address="Sidi Gaber",ImageName=("padel West.jpg"),Capacity=4,Price=210,Description="Smouha",ImagePath=Path.Combine("images","padel West.jpg")},
                        new Place() { PlaceName = "White Padel", Address="Kafr Abdo",ImageName=("White Padel.jpg"),Capacity=2,Price=250,Description="Smouha",ImagePath=Path.Combine("images","White Padel.jpg")},
                        new Place() { PlaceName = "Padel Box",Address="Gleem",ImageName=("Padel Box.png"),Capacity=4,Price=230,Description="Smouha",ImagePath=Path.Combine("images","Padel Box.png")},
                        new Place() { PlaceName = "We Padel", Address="Roshdy",ImageName=("We Padel.jpg"),Capacity=4,Price=230,Description="Smouha",ImagePath=Path.Combine("images","We Padel.jpg")},
                        new Place() { PlaceName = "Top Padel", Address="Miami",ImageName=("Top Padel.jpg"),Capacity=4,Price=170,Description="Smouha",ImagePath=Path.Combine("images","Top Padel.jpg")},
                        new Place() { PlaceName = "Padel Court", Address="Bab Sharqi",ImageName=("Padel Court.jpg"),Capacity=8,Price=200,Description="Smouha",ImagePath=Path.Combine("images","Padel Court.jpg")},
                        new Place() { PlaceName = "Padel Sport",Address="Stanley",ImageName=("Padel Sport.jpg"),Capacity=6,Price=240,Description="Smouha",ImagePath=Path.Combine("images","Padel Sport.jpg")},
                   });
                }

                if (!db.Venues.Any())
                {
                    db.Venues.AddRange(new List<Venue>()
                    {
                        new Venue() { Name = "Antonyades"        , Location="Smouha"     , Capacity=30000 ,   Description="Concerts in the marvelous gardens of Antonyades"         , Image="/uploads/venues/Antonyades.jpg"},
                        new Venue() { Name =  "Golden Jewel"     , Location="Sidi Gaber" , Capacity=50000 ,   Description="The sea is the best place to throw a concert"            , Image="/uploads/Venues/GoldenJewel.jpg"},
                        new Venue() { Name = "Alexandria Stadium", Location="ElShatbi"   , Capacity=25000 ,   Description="Feeling short? In the stadium you can see from anywhere!", Image="/uploads/Venues/AlexStad.jpg"},

                   });
                    db.SaveChanges();
                }
                if (!db.Concerts.Any())
                {
                    var venue1 = db.Venues.FirstOrDefault(v => v.Name == "Antonyades");
                    var venue2 = db.Venues.FirstOrDefault(v => v.Name == "Golden Jewel");
                    var venue3 = db.Venues.FirstOrDefault(v => v.Name == "Alexandria Stadium");
                    db.Concerts.AddRange(new List<Concert>()
                    {
                        new Concert() { Name = "Graduation 2024"    , Organizer="The Bomb"     , Artist="Tamer Hosni"             , Date= DateTime.Parse("2024-10-31 03:42:00.0000000") ,  Description="Graduation Event for Students"  , Image="/images/concerts/Grad1.jpg"  ,VenueId= venue1.VenueId },
                        new Concert() { Name = "Rap Legends"        , Organizer="Music Arena"  , Artist="Lege-cy X Shehab X Zaza" , Date= DateTime.Parse("2024-10-31 03:42:00.0000000") ,  Description="Exclusive Event for Rap lovers" , Image="/images/concerts/raplegends.jpg",VenueId= venue1.VenueId }


                   });
                    db.SaveChanges();
                }

                if (!db.TicketTypes.Any())
                {
                    var concert1 = db.Concerts.FirstOrDefault(c => c.Name == "Graduation 2024");
                    var concert2 = db.Concerts.FirstOrDefault(c => c.Name == "Rap Legends");

                    db.TicketTypes.AddRange(new List<TicketType>()
                    {
                        new TicketType() { TypeName = "Regular"        , Price=300     , Description="Economic"               ,AvailableTickets=5000 ,  ConcertId=concert1.ConcertId    , Image=Path.Combine("images","Ticket11.jpg"), },
                        new TicketType() { TypeName = "VIP"            , Price=550     , Description="First rows"             ,AvailableTickets=1000 ,  ConcertId=concert1.ConcertId    , Image=Path.Combine("images","Ticket12.jpg"), },
                        new TicketType() { TypeName = "Fan Pit"        , Price=1000    , Description="As close as you can be" ,AvailableTickets=50   ,  ConcertId=concert1.ConcertId    , Image=Path.Combine("images","Ticket13.jpg"), },

                        new TicketType() { TypeName = "Regular"        , Price=400     , Description="Economic"               ,AvailableTickets=6000 ,  ConcertId=concert2.ConcertId    , Image=Path.Combine("images","Ticket11.jpg"), },
                        new TicketType() { TypeName = "VIP"            , Price=600     , Description="First rows"             ,AvailableTickets=1500 ,  ConcertId=concert2.ConcertId    , Image=Path.Combine("images","Ticket12.jpg"), },
                        new TicketType() { TypeName = "Fan Pit"        , Price=1500    , Description="As close as you can be" ,AvailableTickets=100  ,  ConcertId=concert2.ConcertId    , Image=Path.Combine("images","Ticket13.jpg"), }



                   });

                    db.SaveChanges();
                }
            }
        }
    }
}