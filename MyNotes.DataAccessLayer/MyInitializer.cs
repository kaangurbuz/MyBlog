using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNotes.EntityLayer;

namespace MyNotes.DataAccessLayer
{
    public class MyInitializer:CreateDatabaseIfNotExists<MyNoteContext>
    {
        protected override void Seed(MyNoteContext context)
        {
            try
            {
                MyNotesUser admin = new MyNotesUser()
                {
                    Name = "Kaan",
                    LastName = "Gurbuz",
                    Email = "kaang@gmail.com",
                    //ActivateGuid = Guid .NewGuid(),
                    IsActive = true,
                    IdAdmin = true,
                    UserName = "admin",
                    Password = "123456",
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUserName = "system"
                };

                //context.SaveChanges();
                MyNotesUser stdUser = new MyNotesUser()
                {
                    Name = "Lidia",
                    LastName = "Mazur",
                    Email = "lidmaz@gmail.com",
                    //ActivateGuid = Guid .NewGuid(),
                    IsActive = true,
                    IdAdmin = false,
                    UserName = "lidia",
                    Password = "123456",
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUserName = "system"
                };
                context.MyNotesUsers.Add(admin);
                context.MyNotesUsers.Add(stdUser);
                for (int i = 0; i < 8; i++)
                {
                    MyNotesUser myUser = new MyNotesUser()
                    {
                        Name = FakeData.NameData.GetFirstName(),
                        LastName = FakeData.NameData.GetSurname(),
                        Email = FakeData.NetworkData.GetEmail(),
                        //ActivateGuid = Guid .NewGuid(),
                        IsActive = true,
                        IdAdmin = true,
                        UserName = $"user-{i}",
                        Password = "1234",
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2),
                            DateTime.Now.AddYears(-1)),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUserName = $"user-{i}"
                    };
                    context.MyNotesUsers.Add(myUser);
                }

                context.SaveChanges();
                List<MyNotesUser> userList = context.MyNotesUsers.ToList();
                for (int i = 0; i < 10; i++)
                {
                    //Adding Categories
                    Category cat = new Category()
                    {
                        Title = FakeData.PlaceData.GetStreetName(),
                        Description = FakeData.PlaceData.GetAddress(),
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2),
                            DateTime.Now.AddYears(-1)),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUserName = "system"
                    };
                    context.Categories.Add(cat);
                    //Adding Notes
                    for (int j = 0; j < FakeData.NumberData.GetNumber(5, 9); j++)
                    {
                        MyNotesUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Note note = new Note
                        {
                            Title = FakeData.PlaceData.GetCity(),
                            Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                            IsDraft = false,
                            LikeCount = FakeData.NumberData.GetNumber(1, 9),
                            Owner = owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2),
                                DateTime.Now.AddYears(-1)),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUserName = owner.UserName
                        };

                        //context.Notes.Add(note);
                        cat.Notes.Add(note);
                        //Adding Comment
                        for (int k = 0; k < FakeData.NumberData.GetNumber(3, 5); k++)
                        {
                            MyNotesUser commentOwner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                            Comment comment = new Comment()
                            {
                                Text = FakeData.TextData.GetSentence(),
                                Owner = commentOwner,
                                //Note = note,
                                CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2),
                                    DateTime.Now.AddYears(-1)),
                                ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                                ModifiedUserName = commentOwner.UserName
                            };
                            note.Comments.Add(comment);
                        }

                        //Adding Like
                        for (int k = 0; k < note.LikeCount; k++)
                        {
                            Liked liked = new Liked()
                            {
                                LikedUser = userList[k]
                                //Note = note
                            };
                            note.Likeds.Add(liked);
                        }
                    }
                }

                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine(
                        "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                throw;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw;
            }
        }
    }
}
