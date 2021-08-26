using System.Collections.Generic;
using Persistance.DTOs;
using System.Linq;
using System.Threading.Tasks;
using Persistance.Converters;
using Microsoft.EntityFrameworkCore;

namespace TaskListAPI.Controllers.Enterprise
{
    public class ItemEC : BaseEC
    {
        public ItemDTO Get(int id)
        {
            using var db = new DataContext();
            return (db.Item.Where(item => item.Id == id).Take(1) as ItemDB).DTO();
        }

        public List<ItemDTO> GetByListId(int id)
        {
            using var db = new DataContext();
            return (db.Item.Where(item => item.ListId == id).Select(i => i.DTO())).ToList();
        }

        public async Task<ItemDTO> AddAsync(ItemDTO itemDto)
        {
            using var db = new DataContext();
            var itemDb = new ItemDB(itemDto)
            {
                Discriminator = itemDto is TaskDTO ? "Task" : "Appointment"
            };
            await db.Item.AddAsync(itemDb);
            await db.SaveChangesAsync();
            return itemDb.DTO();
        }

        public async Task<ItemDTO> UpdateAsync(ItemDTO itemDto)
        {
            using var db = new DataContext();
            var old = db.Item.FirstOrDefault(i => i.Id == itemDto.Id);
            if (old is null) return null;

            db.Item.Remove(old);

            var itemDb = new ItemDB(itemDto)
            {
                Discriminator = itemDto is TaskDTO ? "Task" : "Appointment"
            };
            await db.Item.AddAsync(itemDb);
            await db.SaveChangesAsync();
            return itemDb.DTO();
        }

        public List<AttendeeDB> AddAttendees(int id, List<AttendeeDB> attendees)
        {
            using var db = new DataContext();
            foreach (var attendee in attendees)
            {
                // Check if attendee exists.
                var check = db.Attendee.Where(a => a.Id == attendee.Id)?.Take(1);
                if (check != null)
                {
                    continue;
                }

                // Add attendee.
                var added = db.Attendee.Add(attendee);

                // Add union.
                db.Attendance.Add(new AttendanceDB() { ItemId = id, AttendeeId = added.Entity.Id });
            }

            db.SaveChanges();

            return db.Attendance.Where(a => a.ItemId == id)?
                .Select(at => db.Attendee.FirstOrDefault(att => att.Id == at.AttendeeId))
                .ToList();
        }

        public ItemDTO Delete(int id)
        {
            using var db = new DataContext();
            var item = db.Item.FirstOrDefault(i => i.Id == id);
            if (item is null) return null;

            db.Item.Remove(item);
            db.SaveChanges();
            return item.DTO();
        }
    }
}
