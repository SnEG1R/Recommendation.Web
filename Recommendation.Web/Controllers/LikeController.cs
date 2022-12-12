﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.CQs.Like.Commands;
using Recommendation.Web.Models.Like;

namespace Recommendation.Web.Controllers;

[Route("api/likes")]
public class LikeController : BaseController
{
    public LikeController(IMapper mapper, IMediator mediator) : base(mapper, mediator)
    {
    }

    [HttpPost]
    public async Task<ActionResult> SetLike([FromBody] SetLikeVm setLikeVm)
    {
        var setLikeCommand = Mapper.Map<SetLikeCommand>(setLikeVm);
        setLikeCommand.UserId = UserId;
        await Mediator.Send(setLikeCommand);

        return Ok();
    }
}