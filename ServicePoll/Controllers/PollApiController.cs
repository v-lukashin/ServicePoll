using ServicePoll.Models;
using ServicePoll.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ServicePoll.Controllers
{
    [RoutePrefix("api/poll")]
    public class PollApiController : ApiController
    {
        private readonly RepositoryGeneric<Poll> _pollRepository;

        public PollApiController(RepositoryGeneric<Poll> pollRep)
        {
            _pollRepository = pollRep;
        }

        #region REST
        //[Route("{id}")]
        //public IHttpActionResult Get(string id)
        //{
        //    return Ok<Poll>(_rep.Get(id));
        //}

        public IHttpActionResult GetAllActive()
        {
            return Ok<IEnumerable<Poll>>(_pollRepository.GetAll().Where(x=>x.IsActive == true));
        }

        //public IHttpActionResult Post(Poll value)
        //{
        //    Poll res = _rep.Add(value);
        //    return Created<Poll>("", res);
        //}

        //private IHttpActionResult Put(string id, Poll value)
        //{
        //    _rep.Update(id, value);
        //    return Ok();
        //}

        //private IHttpActionResult Delete(string id)
        //{
        //    _rep.Remove(id);
        //    return Ok();
        //}
        #endregion
    }
}