using DotNet5_web.API.Logging.Interface;
using DotNet5_web.Core.Domain.Interface;
using DotNet5_web.Core.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace DotNet5_web.API.Controllers
{
    [ServiceFilter(typeof(ILogRepository))]
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private IDomain iDomain;
        private ILogger<Enterprise_MVC_CoreDTO> _logger;

        public DemoController(IDomain iDomain, ILogger<Enterprise_MVC_CoreDTO> logger)
        {
            this.iDomain = iDomain;
            this._logger = logger;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Enterprise_MVC_CoreDTO), (int)HttpStatusCode.OK)]
        public IEnumerable<Enterprise_MVC_CoreDTO> GetAll()
        {
            _logger.LogInformation("Executing Demo controller - Get method without authorize.");
            iDomain.DomainExecute(out IEnumerable<Enterprise_MVC_CoreDTO> returnObj);
            if (returnObj is not null)
            {
                return returnObj;
            }
            return new List<Enterprise_MVC_CoreDTO>();
        }
    }
}
