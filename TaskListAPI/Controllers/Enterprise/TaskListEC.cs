using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistance.Converters;

namespace TaskListAPI.Controllers.Enterprise
{
    public class TaskListEC : BaseEC
    {
        public static TaskListDB Get(int id)
        {
            using var db = new DataContext();
            return db.TaskList.FirstOrDefault(tl => tl.Id == id);
        }

        public TaskListDB GetDefault()
        {
            using var db = new DataContext();
            if (db.TaskList.Any())
            {
                return db.TaskList.FirstOrDefault();
            }

            return null;
        }

        public List<TaskListDB> GetAll()
        {
            using var db = new DataContext();
            // Return all list names.
            try
            {
                return db.TaskList.Where(tl => true).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<TaskListDB> AddAsync(TaskListDB taskList)
        {
            using var db = new DataContext();
            db.TaskList.Add(
                new TaskListDB
                {
                    Id = taskList.Id,
                    Name = taskList.Name,
                    Description = taskList.Description
                });
                
            await db.SaveChangesAsync();
            return taskList;
        }

        public TaskListDB Add(string name, string description)
        {
            using var db = new DataContext();
            var list = new TaskListDB() { Id = 0, Name = name, Description = description };
            db.TaskList.Add(list);
            db.SaveChanges();
            return list;
        }

        public TaskListDB Update(TaskListDB taskList)
        {
            using var db = new DataContext();
            var old = db.TaskList.FirstOrDefault(tl => tl.Id == taskList.Id);
            db.TaskList.Remove(old);
            db.TaskList.Add(taskList);
            db.SaveChanges();
            return taskList;
        }

        public TaskListDB Delete(int id)
        {
            var itemEc = new ItemEC();
            var items = itemEc.GetByListId(id);
            foreach (var item in items)
            {
                itemEc.Delete(item.Id);
            }

            using var db = new DataContext();
            var taskList = db.TaskList.FirstOrDefault(tl => tl.Id == id);
            db.TaskList.Remove(taskList);
            db.SaveChanges();
            return taskList;
        }
    }
}
