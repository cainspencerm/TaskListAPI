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
    public class AttendanceEC : BaseEC
    {
        public List<AttendeeDB> GetByItemId(int id)
        {
            var attendees = new List<AttendeeDB>();
            using (var db = new DataContext(new DbContextOptions<DataContext>()))
            {
                var attendance = db.Attendance.Where(i => i.ItemId == id).ToList();
                foreach (var attendee in attendance)
                {
                    attendees.Add(db.Attendee.Where(person => person.Id == attendee.AttendeeId)?.Take(1) as AttendeeDB);
                }
            }

            return attendees;
        }

        public AttendanceDB Add(AttendanceDB attendance)
        {
            using var db = new DataContext(new DbContextOptions<DataContext>());
            db.Attendance.Add(attendance);
            db.SaveChanges();
            return attendance;
        }

        public AttendanceDB Add(int itemId, int attendeeId)
        {
            using var db = new DataContext(new DbContextOptions<DataContext>());
            var attendance = new AttendanceDB() { ItemId = itemId, AttendeeId = attendeeId };
            db.Attendance.Add(attendance);
            db.SaveChanges();
            return attendance;
        }

        public List<AttendanceDB> DeleteByItemId(int id)
        {
            var attendances = new List<AttendanceDB>();
            using (var db = new DataContext(new DbContextOptions<DataContext>()))
            {
                var attendance = db.Attendance.Where(i => i.ItemId == id).ToList();
                foreach (var value in attendance)
                {
                    attendances.Add(value);
                    db.Attendance.Remove(value);
                }
            }

            return attendances;
        }
    }
}
