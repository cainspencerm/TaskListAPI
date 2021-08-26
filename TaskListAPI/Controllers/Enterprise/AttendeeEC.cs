using Persistance.Converters;
using Persistance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskListAPI.Controllers.Enterprise
{
    public class AttendeeEC : BaseEC
    {
        public AttendeeDB Add(AttendeeDB attendee)
        {
            using var db = new DataContext(new DbContextOptions<DataContext>());
            db.Attendee.Add(attendee);
            db.SaveChanges();
            return attendee;
        }

        public AttendeeDB Add(string email, string firstName, string lastName)
        {
            using var db = new DataContext(new DbContextOptions<DataContext>());
            var attendee = new AttendeeDB() { Email = email, FirstName = firstName, LastName = lastName };
            db.Attendee.Add(attendee);
            db.SaveChanges();
            return attendee;
        }

        public AttendeeDB Update(AttendeeDB attendee)
        {
            using var db = new DataContext(new DbContextOptions<DataContext>());
            var old = db.Attendee.FirstOrDefault(a => a.Id == attendee.Id);
            db.Attendee.Remove(old);
            db.Attendee.Add(attendee);
            db.SaveChanges();
            return attendee;
        }

        public AttendeeDB Delete(int id)
        {
            using var db = new DataContext(new DbContextOptions<DataContext>());
            var attendee = db.Attendee.FirstOrDefault(a => a.Id == id);
            db.Attendee.Remove(attendee);
            db.SaveChanges();
            return attendee;
        }
    }
}
