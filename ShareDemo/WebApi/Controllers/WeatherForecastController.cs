﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using WebApi.DTOs;
using WebApi.Events;
using WebApi.Services;
using IdentityModel.Client;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IValuesService _valuesService;

        #region 属性注入
        public Values2Service _values2Service { get; set; } 
        public Values3Service _values3Service { get; set; }
        #endregion

        public WeatherForecastController(IMapper mapper,IMediator mediator, IValuesService valuesService)
        {
            _mapper = mapper;
            _mediator = mediator;
            _valuesService = valuesService;
        }

        #region AutoMapper测试
        [HttpGet("stores/{storeId}")]
        public ActionResult<StoreDTO> Store([FromRoute]string storeId)
        {
            var storeEntity = new BasStore
            {
                Pid = storeId,
                Tilte = "美程酒店",
                Address = "苏州市XXXXXXX"
            };

            var storeDTO = _mapper.Map<StoreDTO>(storeEntity);
            return Ok(storeDTO);
        }

        [HttpGet("serviceOrders/{serviceOrderId}")]
        public ActionResult<ServiceOrderDTO> ServiceOrder([FromRoute]string serviceOrderId)
        {
            var serviceOrderEntity = new MinServiceOrder
            {
                Pid = serviceOrderId,
                Type = 1
            };
            var invoiceTitleEntity = new MinInvoiceTitle
            {
                Name = "这是发票抬头名称",
                Pid = Guid.NewGuid().ToString()
            };

            var serviceOrderDTO = _mapper.Map<ServiceOrderDTO>(serviceOrderEntity);
            serviceOrderDTO.TitleInfo = _mapper.Map<InvoiceTitleDTO>(invoiceTitleEntity);

            return Ok(serviceOrderDTO);
        }
        #endregion

        #region MediatR测试
        [HttpGet("serviceOrders/newServiceOrder")]
        public async Task<ActionResult<string>> NewServiceOrderAsync(string description)
        {
            var saveResult = await _mediator.Send(new NewServiceOrderEvent(description));
            return Ok(saveResult ? "预约服务工单成功" : "预约服务工单失败");
        }
        #endregion

        #region FluentValidation测试
        [HttpPost("stores")]
        public ActionResult<bool> UpdateStore([FromBody]StoreDTO storeDTO)
        {
            return Ok(true);
        }

        [HttpPost("stores/add")]
        public ActionResult<bool> AddStore([FromBody]StoreAddDTO storeDTO)
        {
            return Ok(true);
        }
        #endregion

        #region Autofac测试
        [HttpGet("value/list")]
        public IEnumerable<string> Get()
        {
            var valueResult = _valuesService.FindAll();
            var value2Result = _values2Service.FindAll();
            var value3Result = _values3Service.FindAll();

            var value4Result = ServiceLocator._lifetimeScope.Resolve<IValues2Service>(new NamedParameter("type", "4")).FindAll();
            var value5Result = ServiceLocator._lifetimeScope.Resolve<IValues2Service>(new NamedParameter("type", "5")).FindAll();

            var value6Instance1 = ServiceLocator._lifetimeScope.Resolve<Values6Service>();
            var value6Instance2 = ServiceLocator._lifetimeScope.Resolve<Values6Service>();
            var IsEqual = Equals(value6Instance1, value6Instance2);

            var boolResult = ServiceLocator._lifetimeScope.Resolve<CallResult<bool>>();

            var value4Result2 = ServiceLocator._lifetimeScope.ResolveNamed<IValues2Service>("4").FindAll();
            var value5Result2 = ServiceLocator._lifetimeScope.ResolveNamed<IValues2Service>("5").FindAll();

            return _valuesService.FindAll();
        }

        [HttpGet("value/{id}")]
        public string Get([FromRoute]int id)
        {
            return _valuesService.Find(id);
        } 


        #endregion

        #region IdentityServer4测试
        [Authorize]
        [HttpGet]
        public IActionResult GetClaims()
        {
            var data = from c in User.Claims select new { c.Type, c.Value };
            var dict = new Dictionary<string, string>();
            foreach (var x in data.ToList())
            {
                dict[x.Type] = x.Value;
            }
            return Ok(dict);
        }

        [AllowAnonymous]
        [HttpGet("RevokeToken")]
        public async Task<ActionResult<bool>> RevokeTokenAsync(string token)
        {
            var client = new HttpClient();

            var result = await client.RevokeTokenAsync(new TokenRevocationRequest
            {
                Address = "http://localhost:50024/connect/revocation",
                ClientId = "hms_client",
                ClientSecret = "123456",
                Token = token
            });

            return Ok(result.IsError);
        } 
        #endregion
    }
}
