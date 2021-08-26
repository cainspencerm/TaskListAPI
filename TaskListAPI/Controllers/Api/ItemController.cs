using Persistance.DTOs;
using Persistance.Converters;
using Persistance.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskListAPI.Controllers.Api;

namespace TaskListAPI.Controllers.Enterprise
{
    [Route("api/Item")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        [HttpGet("Test")]
        public ActionResult<bool> Test()
        {
            return Ok(true);
        }

        [HttpGet("Get")]
        public ActionResult<List<ItemDTO>> Get()
        {
            var list = TaskListController.TaskList;
            if (list == null) return null;

            var items = new ItemEC().GetByListId(list.Id);
            return Ok(items);
        }

        [HttpGet("GetAll")]
        public ActionResult<List<ItemDTO>> GetAll([FromQuery] int listId)
        {
            var results = new ItemEC().GetByListId(listId);
            if (results != null)
            {
                return Ok(results);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("AddOrUpdate")]
        public ActionResult<ItemDTO> AddOrUpdate([FromBody] ItemDTO dto)
        {
            if (dto == null) return BadRequest();

            if (dto.Id == 0)
            {
                dto = new ItemEC().AddAsync(dto).Result;
            }
            else
            {
                dto = new ItemEC().UpdateAsync(dto).Result;
            }

            return Ok(dto);
        }

        [HttpPost("AddAttendees")]
        public ActionResult<List<AttendeeDB>> AddAttendees([FromQuery] int id, [FromBody] List<AttendeeDB> attendees)
        {
            return Ok(new ItemEC().AddAttendees(id, attendees));
        }

        [HttpPost("Delete")]
        public ActionResult<ItemDTO> Delete([FromBody] int id)
        {
            return Ok(new ItemEC().Delete(id));
        }

        [HttpGet("Search")]
        public ActionResult<List<ItemDTO>> Search([FromQuery] string search)
        {
            var suitableItems = new List<ItemDTO>();
            var splitText = search.Trim().ToLower().Split(" ");
            foreach (var item in new ItemEC().GetByListId(TaskListController.TaskList.Id))
            {
                var found = splitText.All((key) =>
                {
                    if (item is TaskDTO)
                    {
                        return (item as TaskDTO).Name.ToLower().Contains(key) ||
                               (item as TaskDTO).Description.ToLower().Contains(key);
                    }
                    else if (item is AppointmentDTO)
                    {
                        return (item as AppointmentDTO).Name.ToLower().Contains(key) ||
                               (item as AppointmentDTO).Description.ToLower().Contains(key) ||
                               string.Join(" ", (item as AppointmentDTO).Attendees).ToLower().Contains(key);
                    }
                    else
                    {
                        return false;
                    }
                });

                if (found)
                {
                    suitableItems.Add(item);
                }
            }

            if (suitableItems.Count == 0) return NotFound();

            return Ok(suitableItems);
        }
    }
}
