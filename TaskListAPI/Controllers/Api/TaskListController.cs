using Persistance.DTOs;
using Persistance.Converters;
using Persistance.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskListAPI.Controllers.Enterprise;

namespace TaskListAPI.Controllers.Api
{
    [ApiController]
    [Route("api/List")]
    public class TaskListController : Controller
    {
        private static TaskListDB _TaskList;
        public static TaskListDB TaskList {
            get
            {
                if (_TaskList is null)
                {
                    try
                    {
                        _TaskList = new TaskListEC().GetDefault();
                    }
                    catch (Exception)
                    {
                        _TaskList = null;
                    }
                }
                return _TaskList;
            }
            set
            {
                _TaskList = value;
            }
        }

        [HttpGet("Test")]
        public ActionResult<bool> Test()
        {
            return Ok(true);
        }

        [HttpGet("GetAll")]
        public ActionResult<TaskListDB> GetAll()
        {
            return Ok(new TaskListEC().GetAll());
        }

        [HttpGet("Get")]
        public ActionResult<string> Get()
        {
            return Ok(TaskList);
        }

        [HttpGet("GetNames")]
        public ActionResult<List<string>> GetNames()
        {
            return Ok(new TaskListEC().GetAll());
        }

        [HttpPost("Create")]
        public ActionResult<bool> Create([FromBody] string name)
        {
            var taskList = new TaskListEC().AddAsync(new TaskListDB() { Name = name, Description = "" }).Result;
            return Ok(taskList);
        }

        [HttpPost("Change")]
        public ActionResult<TaskListDB> Change([FromBody] int id)
        {
            TaskList = TaskListEC.Get(id);

            return Ok(TaskList);
        }

        [HttpPost("Delete")]
        public ActionResult<List<ItemDTO>> Delete([FromBody] int id)
        {
            var list = new ItemController().GetAll(id);
            new TaskListEC().Delete(id);

            if (TaskList?.Id == id)
            {
                TaskList = new TaskListEC().GetDefault();
            }

            return Ok(list);
        }
    }
}
