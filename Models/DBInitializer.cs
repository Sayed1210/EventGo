using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EventGo.Models
{
    public class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
    public class DBInitializer
    {
        public static async Task<byte[]> ImageConverter(string imageName)
        {
            string path = "./wwwroot/images/" + imageName;
            var stream = File.OpenRead(path);
            var formfile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
            using (var _stream = new MemoryStream())
            {
                await formfile.CopyToAsync(_stream);
                return _stream.ToArray();
            }
        }

        


        public static async Task SeedDB(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<MovieContext>();

                await db.Database.EnsureCreatedAsync();


                //Seeding Categories
                if (!db.Categories.Any())
                {
                    db.Categories.AddRange(new List<Category>()
                    {
                        new Category() { Name = "Comdey", Image= await ImageConverter("Comedy.jpg")},
                        new Category() { Name = "Romance", Image= await ImageConverter("Romance.jpg")},
                        new Category() { Name = "Drama", Image= await ImageConverter("Drama.jpg")},
                        new Category() { Name = "Action", Image= await ImageConverter("Action.jpg")},
                        new Category() { Name = "Thriller", Image= await ImageConverter("Thriller.jpg")},
                        new Category() { Name = "Horror", Image= await ImageConverter("Horror.jpg")},
                        new Category() { Name = "Animation", Image= await ImageConverter("Animation.jpg")},
                        new Category() { Name = "Adventure", Image= await ImageConverter("Adventure.jpg")},
                        new Category() { Name = "Documentary", Image= await ImageConverter("Documentary.jpg")},
                    });
                }

                //Adding Cinemas

                if (!db.Cinemas.Any())
                {
                    db.Cinemas.AddRange(new List<Cinema>()
                    {
                        new Cinema() { Name ="IMax Americana", Location ="Giza", Image= await ImageConverter("/Cinema/Galaxy.jpg"), Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},
                        new Cinema() { Name ="Point 90 Cinema", Location ="Cairo", Image= await ImageConverter("/Cinema/vox.jpg"), Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},
                        new Cinema() { Name ="Vox Cinema", Location ="Cairo", Image= await ImageConverter("/Cinema/Galaxy.jpg"), Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},
                        new Cinema() { Name ="Renaissance Cinema St. Stefano", Image= await ImageConverter("/Cinema/vox.jpg"), Location ="Alexandria", Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."},
                    });
                }


               // Adding Actors
                if (!db.Actors.Any())
                {
                    db.Actors.AddRange(new List<Actor>()
                    {
                        new Actor()
                        {
                            Name ="Matt LeBlanc",
                            Image = await ImageConverter("actor-1.jpeg"),
                            Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                        },
                        new Actor()
                        {
                            Name ="Chris Tucker",
                            Image = await ImageConverter("actor-2.jpeg"),
                            Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                        },
                        new Actor()
                        {
                            Name ="Angelina Jolie",
                            Image = await ImageConverter("actor-3.jpeg"),
                            Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                        },
                        new Actor()
                        {
                            Name ="Jim Carrey",
                            Image = await ImageConverter("actor-4.jpeg"),
                            Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                        },
                        new Actor()
                        {
                            Name ="Will Smith",
                            Image = await ImageConverter("actor-5.jpeg"),
                            Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                        },
                        new Actor()
                        {
                            Name ="Dwayne Johnson",
                            Image = await ImageConverter("actor-6.jpg"),
                            Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                        },
                        new Actor()
                        {
                            Name ="Jason Statham",
                            Image = await ImageConverter("actor-7.jpg"),
                            Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                        },

                    });
                }

                if (!db.Producers.Any())
                {
                    db.Producers.AddRange(new List<Producer>()
                        {
                            new Producer()
                            {
                                Name = "Kevin Feige",
                                Image = await ImageConverter("producer_1.jpg"),
                                Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                            },
                            new Producer()
                            {
                                Name = "Quantin Tarantino",
                                Image = await ImageConverter("producer_2.jpg"),
                                Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                            },
                        });
                }
                db.SaveChanges();


                //Adding Movies Here

                if (!db.Movies.Any())
                    {
                    //start of adding movie
                        Guid guid1 = Guid.NewGuid();
                        var movie1 = new Movie()
                        {

                            Id = guid1,
                            Name = "عسل اسود 2010",
                            StartDate = new DateTime(2024, 1, 1),
                            EndDate = new DateTime(2025, 1, 1),
                            Rate = 8,
                            Trailer= "https://youtube.com/watch?v=XaUIgeddM6U&t=3802s",
                            Price = 120,
                            Description = "The film talks about “Egyptian Sayed El Araby” (Ahmed Helmy), who left Egypt and immigrated to America with his parents when he was ten years old. Twenty years later, he returns to his country with a desire to settle there after the death of his parents. He faces many paradoxes resulting from differences in time and cultures, as he holds a passport. An American travels, which makes everyone treat him well until he is exposed to an accident in which he loses this passport, so everyone’s treatment of him changes to the opposite, and he decides to rebel against this situation..",
                            Producer_Id = 2,
                            Cat_Id = 1,
                            Image = await ImageConverter("movie-4.jpg"),
                        };
                        db.Movies.Add(movie1);
                    var actors1 = new List<int> { 1, 3, 5 };
                    foreach (var id in actors1)
                    {
                        db.MovieActors.Add(new MovieActor()
                        {
                            MovieId = guid1,
                            ActorId = id
                        });
                    }
                    var cinemas1 = new List<int> { 1, 3, 4 };
                    var quantities1 = new List<int> { 50, 40, 30 };

                        //adding to cinema movies table
                        for (var i = 0; i < cinemas1.Count; i++)
                        {
                            db.MovieInCinemas.Add(new MovieInCinema()
                            {
                                Quantity = quantities1[i],
                                MovieId = guid1,
                                CinemaId = cinemas1[i]
                            });
                        }
                        //end of adding one movie
                        
                        //start of adding movie
                        Guid guid2 = Guid.NewGuid();
                        var movie2 = new Movie()
                        {
                            Id = guid2,
                            Name = "كيرة والجن 2022",
                            StartDate = new DateTime(2024, 1, 1),
                            EndDate = new DateTime(2025, 1, 1),
                            Rate = 7,
                            Trailer= "https://youtu.be/igChw42zmP0?si=ZHhMo7YoOlpQ94iG",
                            Price = 150,
                            Description = "The film monitors the state of turmoil that was raging in the Egyptian street coinciding with the outbreak of the 1919 Revolution, which was the major event that united the destinies of Ahmed Abdel-Hay Kira and Abdel-Qader El-Jin to participate in the struggle against the British occupier.",
                            Producer_Id = 1,
                            Cat_Id = 6,
                            Image = await ImageConverter("movie-1.jpg"),
                        };
                        db.Movies.Add(movie2);
                    var actors2 = new List<int> { 2, 4, 5 };
                    foreach (var id in actors2)
                    {
                        db.MovieActors.Add(new MovieActor()
                        {
                            MovieId = guid2,
                            ActorId = id
                        });
                    }
                    var cinemas2 = new List<int> { 2, 4 };
                    var quantities2 = new List<int> { 40, 60 };

                        //adding to cinema movies table
                        for (var i = 0; i < cinemas2.Count; i++)
                        {
                            db.MovieInCinemas.Add(new MovieInCinema()
                            {
                                Quantity = quantities2[i],
                                MovieId = guid2,
                                CinemaId = cinemas2[i]
                            });
                        }
                    //end of adding one movie

                    //start of adding movie
                    Guid guid3 = Guid.NewGuid();
                    var movie3 = new Movie()
                    {
                        Id = guid3,
                        Name = "الجزيرة 2007",
                        StartDate = new DateTime(2024, 1, 1),
                        EndDate = new DateTime(2025, 1, 1),
                        Rate = 7,
                        Trailer = "https://youtu.be/riGEa-CZB24?si=5QGy8Fy7JqUENx1G",
                        Price = 150,
                        Description = "Al Jazeera is an Egyptian action and biographical film directed by Sherif Arafa, written by Mohamed Diab, and starring Ahmed El Sakka, Hend Sabry, Khaled El Sawy, and Mahmoud Yassin. The events of the film are real and are inspired by the story of Ezzat Hanafi, an Assiut drug and weapons dealer who was allied with the government in order to eliminate terrorists in Egypt, but the situation develops and the issue gets out of control and the government and police turn against him.",
                        Producer_Id = 2,
                        Cat_Id = 4, 
                        Image = await ImageConverter("movie-2.jpg"),
                    };
                    db.Movies.Add(movie3);
                    var actors3 = new List<int> { 1, 2, 3 };
                    foreach (var id in actors3)
                    {
                        db.MovieActors.Add(new MovieActor()
                        {
                            MovieId = guid3,
                            ActorId = id
                        });
                    }
                    var cinemas3 = new List<int> { 2, 4 };
                    var quantities3 = new List<int> { 40, 60 };

                    //adding to cinema movies table
                    for (var i = 0; i < cinemas3.Count; i++)
                    {
                        db.MovieInCinemas.Add(new MovieInCinema()
                        {
                            Quantity = quantities3[i],
                            MovieId = guid3,
                            CinemaId = cinemas3[i]
                        });
                    }
                    //end of adding one movie

                    //start of adding movie
                    Guid guid4 = Guid.NewGuid();
                    var movie4 = new Movie()
                    {
                        Id = guid4,
                        Name = "لا تراجع ولا استسلام 2010",
                        StartDate = new DateTime(2024, 1, 1),
                        EndDate = new DateTime(2025, 1, 1),
                        Rate = 7,
                        Trailer = "https://youtu.be/tufxaIXqt0o?si=EtEH5WJgDQts4AuD",
                        Price = 170,
                        Description = "The police resort to “Hazloum” (Ahmed Makki) to replace “Adham,” the right-hand man of “Azzam” (Ezzat Abu Auf), one of the biggest drug dealers in Egypt, to catch him, taking advantage of the great similarity between them. The mission is followed by “Lieutenant Colonel Siraj” (Majed El-Kedwany), and “Jermaine” (Donia Samir Ghanem), Azzam’s secretary, cooperates with him, until Azzam discovers the plan and events follow.",
                        Producer_Id = 2,
                        Cat_Id = 1,
                        Image = await ImageConverter("images (3).jpeg"),
                    };
                    db.Movies.Add(movie4);
                    var actors4 = new List<int> { 1, 2, 3 };
                    foreach (var id in actors4)
                    {
                        db.MovieActors.Add(new MovieActor()
                        {
                            MovieId = guid4,
                            ActorId = id
                        });
                    }
                    var cinemas4 = new List<int> { 2, 4 };
                    var quantities4 = new List<int> { 40, 60 };

                    //adding to cinema movies table
                    for (var i = 0; i < cinemas4.Count; i++)
                    {
                        db.MovieInCinemas.Add(new MovieInCinema()
                        {
                            Quantity = quantities4[i],
                            MovieId = guid4,
                            CinemaId = cinemas4[i]
                        });
                    }
                    //end of adding one movie


                    //start of adding movie
                    Guid guid5 = Guid.NewGuid();
                    var movie5 = new Movie()
                    {
                        Id = guid5,
                        Name = "صعيدى فى الجامعة الأمريكية 1998",
                        StartDate = new DateTime(2024, 1, 1),
                        EndDate = new DateTime(2025, 1, 1),
                        Rate = 8,
                        Trailer = "https://www.youtube.com/watch?https://youtu.be/e66ixho4aDw?si=AUFq5N3HcYwitVE1",
                        Price = 190,
                        Description = "Khalaf Al-Dahshouri (Mohamed Henedy) is a Upper Egypt student who gets first place in high school. He wins the award for admission to the American University in Cairo. He travels and resides there with two of his colleagues. At the university, he gets to know many colleagues and falls in love with his colleague (Abla), whom he used to care about. Her professor at the university, and at the same time she is attracted to him (Sayyada), but he does not feel the same way.",
                        Producer_Id = 2,
                        Cat_Id = 1,
                        Image = await ImageConverter("movie-3.jpg"),
                    };
                    db.Movies.Add(movie5);
                    var actors5 = new List<int> { 1, 4, 3 };
                    foreach (var id in actors5)
                    {
                        db.MovieActors.Add(new MovieActor()
                        {
                            MovieId = guid5,
                            ActorId = id
                        });
                    }
                    var cinemas5 = new List<int> { 2, 4 };
                    var quantities5 = new List<int> { 40, 60 };

                    //adding to cinema movies table
                    for (var i = 0; i < cinemas5.Count; i++)
                    {
                        db.MovieInCinemas.Add(new MovieInCinema()
                        {
                            Quantity = quantities5[i],
                            MovieId = guid5,
                            CinemaId = cinemas5[i]
                        });
                    }
                    //end of adding one movie

                    //start of adding movie
                    Guid guid6 = Guid.NewGuid();
                    var movie6 = new Movie()
                    {
                        Id = guid6,
                        Name = "الفيل الأزرق 2019",
                        StartDate = new DateTime(2024, 1, 1),
                        EndDate = new DateTime(2025, 1, 1),
                        Rate = 8,
                        Trailer = "https://youtu.be/2WhA20_JYsI?si=yXKvkqbWOvTKta0z",
                        Price = 130,
                        Description = "The film follows the life of a psychiatrist named Yahya Rashed (Kareem Abdel Aziz). Yahya is a special kind of doctor who has faced several challenges in his professional and personal life. After being absent from work for five years, he returns to the Abbassia Hospital. He decides to start fresh with new insights when he is asked to write a report about a psychiatric patient. He is shocked to discover that this patient is his old friend, Dr. Sherif Maher El Kordi (Khaled El Sawi). The events become more complex, revealing intriguing secrets about the characters and unveiling a mountain of mystery.",
                        Producer_Id = 2,
                        Cat_Id = 5,
                        Image = await ImageConverter("images (1).jpeg"),
                    };
                    db.Movies.Add(movie6);
                    var actors6 = new List<int> { 1, 6, 5 };
                    foreach (var id in actors6)
                    {
                        db.MovieActors.Add(new MovieActor()
                        {
                            MovieId = guid6,
                            ActorId = id
                        });
                    }
                    var cinemas6 = new List<int> { 1, 4 };
                    var quantities6 = new List<int> { 40, 60 };

                    //adding to cinema movies table
                    for (var i = 0; i < cinemas6.Count; i++)
                    {
                        db.MovieInCinemas.Add(new MovieInCinema()
                        {
                            Quantity = quantities6[i],
                            MovieId = guid6,
                            CinemaId = cinemas6[i]
                        });
                    }
                    //end of adding one movie
                }



                db.SaveChanges();
                }

            }
        


        public static async Task CreateUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                string adminUserEmail = "admin@emovies.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        FullName = "Admin User",
                        UserName = "admin-user",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "InitialAdmin@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }


                string appUserEmail = "user@emovies.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new User()
                    {
                        FullName = "Application User",
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "InitialUser@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }

    }
}
