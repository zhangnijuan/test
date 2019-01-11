using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZnjTest.IBLL;
using ZnjTest.Model.Entities;

namespace ZnjTest.MisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBaseService<AdminEntity> baseService;
        public AdminController(IBaseService<AdminEntity> baseService)
        {
            this.baseService = baseService;
        }
        [Route("/api/[controller]/list")]
        [HttpGet]
        public ActionResult GetList()
        {
            var data = baseService.GetList(i=>true);
            return Ok(new {code = 0, data});
        }
        [Route("/api/[controller]/{action}/{id}")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var entity = baseService.GetList(i => i.Id==id).FirstOrDefault();
            if (entity!=null)
            {
               var res= baseService.DeleteAsync(entity).Result;
            }

            return Ok(new {code = 0});
        }
    }
}