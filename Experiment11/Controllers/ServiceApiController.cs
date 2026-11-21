using SmartCampusPortal.DAL;
using SmartCampusPortal.Models;
using System.Web.Http;

namespace SmartCampusPortal.Controllers
{
    public class ServiceApiController : ApiController
    {
        private readonly ServiceRequestDAL dal = new ServiceRequestDAL();

        // GET: api/ServiceApi
        [HttpGet]
        [Route("api/ServiceApi")]
        public IHttpActionResult GetRequests()
        {
            var list = dal.GetAll();
            return Ok(list);
        }

        // GET: api/ServiceApi/5
        [HttpGet]
        [Route("api/ServiceApi/{id:int}")]
        public IHttpActionResult GetRequestById(int id)
        {
            var req = dal.GetById(id);
            if (req == null)
            {
                return NotFound();
            }
            return Ok(req);
        }

        // POST: api/ServiceApi
        [HttpPost]
        [Route("api/ServiceApi")]
        public IHttpActionResult CreateRequest([FromBody] ServiceRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int newId = dal.Insert(request);
            request.RequestId = newId;

            return Created("api/ServiceApi/" + newId, request);
        }

        // PUT: api/ServiceApi/5
        [HttpPut]
        [Route("api/ServiceApi/{id:int}")]
        public IHttpActionResult UpdateRequest(int id, [FromBody] ServiceRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            request.RequestId = id;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool updated = dal.Update(request);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(new { message = "Service request updated successfully", data = request });
        }

        // DELETE: api/ServiceApi/5
        [HttpDelete]
        [Route("api/ServiceApi/{id:int}")]
        public IHttpActionResult DeleteRequest(int id)
        {
            bool deleted = dal.Delete(id);
            if (!deleted)
            {
                return NotFound();
            }

            return Ok(new { message = "Service request #" + id + " deleted successfully" });
        }

        // PUT: api/ServiceApi/UpdateStatus/1 (Preserved from reference project)
        [HttpPut]
        [Route("api/ServiceApi/UpdateStatus/{id:int}")]
        public IHttpActionResult UpdateStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Status cannot be empty.");
            }

            bool updated = dal.UpdateStatus(id, status, null);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(new { message = "Status updated successfully", requestId = id, newStatus = status });
        }
    }
}
