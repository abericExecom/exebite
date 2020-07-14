﻿using System.Linq;
using Either;
using Exebite.Common;
using Exebite.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Exebite.API.Controllers
{
    [Produces("application/json")]
    [Route("")]
    public class HomeController : ControllerBase
    {
        private readonly ICustomerCommandRepository _commandRepo;
        private readonly ICustomerQueryRepository _queryRepo;
        private readonly IEitherMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ICustomerCommandRepository commandRepo,
            ICustomerQueryRepository queryRepo,
            IEitherMapper mapper,
            ILogger<HomeController> logger)
        {
            _commandRepo = commandRepo;
            _queryRepo = queryRepo;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("signin")]
        [Authorize]
        public IActionResult GoogleSignIn()
        {
            string googleId = User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
            return _queryRepo.Query(new CustomerQueryModel { GoogleUserId = googleId })
                   .Map(x => SignInUser(x, googleId))
                   .Reduce(_ => InternalServerError(), x => _logger.LogError(x.ToString()));
        }

        private IActionResult SignInUser(PagingResult<DomainModel.Customer> customers, string googleId)
        {
            if (!customers.Items.Any())
            {
                return _commandRepo.Insert(
                      new CustomerInsertModel
                      {
                          Balance = 0,
                          GoogleUserId = googleId,
                          Name = User.Claims.FirstOrDefault(x => x.Type.EndsWith("name"))?.Value
                      })
                      .Map(x => AllOk(new { id = x }))
                      .Reduce(_ => InternalServerError(), x => _logger.LogError(x.ToString()));
            }

            return AllOk(new { id = customers.Items.First().Id });
        }
    }
}