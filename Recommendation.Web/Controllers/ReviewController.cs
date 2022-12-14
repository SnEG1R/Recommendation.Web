﻿using System.Collections;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.CQs.Review.Commands.Create;
using Recommendation.Application.CQs.Review.Queries.GetAllReviewByUserId;
using Recommendation.Application.CQs.Review.Queries.GetPageReviews;
using Recommendation.Application.CQs.Review.Queries.GetReview;
using Recommendation.Web.Models.Review;

namespace Recommendation.Web.Controllers;

[ApiController]
[Route("api/reviews")]
[Authorize]
public class ReviewController : BaseController
{
    public ReviewController(IMapper mapper, IMediator mediator)
        : base(mapper, mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] int numberPage, [FromQuery] int pageSize,
        [FromQuery] string? searchText)
    {
        var getPageReviewsQuery = new GetPageReviewsQuery(numberPage, pageSize);
        var getPageReviewsVm = await Mediator.Send(getPageReviewsQuery);

        return Ok(getPageReviewsVm);
    }

    [AllowAnonymous]
    [HttpGet("{reviewId:guid}")]
    public async Task<ActionResult<IEnumerable<GetReviewDto>>> Get(Guid reviewId)
    {
        var getReviewQuery = new GetReviewQuery(UserId, reviewId);
        var review = await Mediator.Send(getReviewQuery);

        return Ok(review);
    }

    [HttpPost, DisableRequestSizeLimit]
    public async Task<ActionResult<Guid>> Create([FromForm] CreateReviewDto reviewDto)
    {
        var createReviewCommand = Mapper.Map<CreateReviewCommand>(reviewDto);
        createReviewCommand.UserId = UserId;
        var reviewId = await Mediator.Send(createReviewCommand);

        return Created("api/reviews", reviewId);
    }

    [HttpGet("get-by-user-id/{userId:guid}")]
    public async Task<ActionResult<IEnumerable
        <GetAllReviewByUserIdDto>>> GetByUserId(Guid userId)
    {
        var getAllReviewByUserIdDtoQuery = new GetAllReviewByUserIdQuery(userId);
        var reviews = await Mediator.Send(getAllReviewByUserIdDtoQuery);

        return Ok(reviews);
    }
}