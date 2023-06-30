using BillingManagementWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BillingManagementWebApp.Data
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BillingManagementDbContext(serviceProvider.GetRequiredService<DbContextOptions<BillingManagementDbContext>>()))
            {
                context.Database.EnsureCreated();
                if (context.Users.Any()||context.Flats.Any()||context.Dues.Any()||context.Invoices.Any()||context.Messages.Any())
                {
                    return;
                }
                var user1 = new User { Name = "Admin", Surname = "Admin", TCNo = 11111111111, Email = "admin@admin.com", PhoneNo = "+111111111111", isAdmin = true, isOwner = true, Password = "admin123", Plate = "34adm111" };
                var user2 = new User { Name = "Owner", Surname = "Owner", TCNo = 22222222222, Email = "owner@owner.com", PhoneNo = "+222222222222", isAdmin = false, isOwner = true, Password = "owner123", Plate = "34own222" };
                var user3 = new User { Name = "tenant", Surname = "tenant", TCNo = 33333333333, Email = "tenant@tenant.com", PhoneNo = "+333333333333", isAdmin = false, isOwner = false, Password = "tenant123", Plate = "34ten333" };
                context.Users.AddRange(user1,user2,user3);
                
                var flat1 = new Flat { BlockName = "A", FloorNo=1, DoorNumber= 1, IsEmpthy=false, Type= "1+1", User= user1 };
                var flat2 = new Flat { BlockName = "B", FloorNo = 2, DoorNumber = 2, IsEmpthy = false, Type = "2+2", User = user2 };
                var flat3 = new Flat { BlockName = "C", FloorNo = 3, DoorNumber = 3, IsEmpthy = false, Type = "3+3", User = user3 };
                var flat4 = new Flat { BlockName = "A", FloorNo = 1, DoorNumber = 2, IsEmpthy = true, Type = "1+1", User = user1 };
                context.Flats.AddRange(flat1,flat2,flat3,flat4);

                var due1 = new Due { Cost = 250, DateCreated = new DateTime(year: 2023, month: 02, day: 01), User = user1, DateDuePaid = new DateTime(year: 2023, month: 04, day: 01) };
                var due2 = new Due { Cost = 320, DateCreated = new DateTime(year: 2023, month: 02, day: 01), User = user1, DateDuePaid = new DateTime(year: 2023, month: 04, day: 01) };
                var due3 = new Due { Cost = 430, DateCreated = new DateTime(year: 2023, month: 02, day: 01),  User = user1 };
                context.Dues.AddRange(due1, due2, due3);

                var invoice1 = new Invoice { Cost=100, DateCreated = new DateTime(2023,2,4), User = user1, Type="electricity", DateInvoicePaid=new DateTime(2023,2,6) };
                var invoice2 = new Invoice { Cost = 50, DateCreated = new DateTime(2023, 2, 9), User = user2, Type = "water" };
                var invoice3 = new Invoice { Cost = 1000, DateCreated = new DateTime(2023, 2, 23), User = user3, Type = "gas", DateInvoicePaid = new DateTime(2023, 2, 27) };
                context.Invoices.AddRange (invoice1, invoice2, invoice3);

                var message1 = new Message { Header = "Slm", Content = "Merhaba!", DateSent = DateTime.Now, Receiver = user1, Sender = user2 };
                var message2 = new Message { Header = "Slm2", Content = "Merhaba!2", DateSent = DateTime.Now, Receiver = user2, Sender = user3 };
                var message3 = new Message { Header = "Slm3", Content = "Merhaba!3", DateSent = DateTime.Now, Receiver = user3, Sender = user1 };
                context.Messages.AddRange (message1, message2, message3);

                context.SaveChanges();
            }
        }
    }
}
