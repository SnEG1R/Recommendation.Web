﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.CQs.Composition.Queries.GetAverageRate;
using Recommendation.Application.CQs.Like.Queries.GetCountLike;
using Recommendation.Application.CQs.Like.Queries.GetIsLike;
using Recommendation.Application.CQs.Rating.Queries.GetOwnSetRating;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Queries.GetReview;

public class GetReviewQueryHandler
    : IRequestHandler<GetReviewQuery, GetReviewDto>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public GetReviewQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper, IMediator mediator)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<GetReviewDto> Handle(GetReviewQuery request,
        CancellationToken cancellationToken)
    {
        var review = await _recommendationDbContext.Reviews
            .Include(r => r.User)
            .Include(r => r.Category)
            .Include(r => r.Composition)
            .Include(r => r.Tags)
            .Include(r => r.Likes)
            .Include(r => r.Composition.Ratings)
            .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);
        var reviewDto = _mapper.Map<GetReviewDto>(review);
        reviewDto.OwnSetRating = await GetOwnSetRating(request.UserId, request.ReviewId);
        reviewDto.IsLike = await GetIsLike(request.UserId, request.ReviewId);
        reviewDto.AverageCompositionRate = await GetAverageRate(request.ReviewId);
        reviewDto.CountLike = await GetCountLike(request.ReviewId);

        return reviewDto;
    }

    private async Task<double> GetAverageRate(Guid reviewId)
    {
        var getAverageRateQuery = new GetAverageRateQuery(reviewId);
        return await _mediator.Send(getAverageRateQuery);
    }

    private async Task<bool> GetIsLike(Guid userId, Guid reviewId)
    {
        var getIsLikeQuery = new GetIsLikeQuery(userId, reviewId);
        return await _mediator.Send(getIsLikeQuery);
    }

    private async Task<int> GetOwnSetRating(Guid userId, Guid reviewId)
    {
        var getOwnSetRatingQuery = new GetOwnSetRatingQuery(userId, reviewId);
        return await _mediator.Send(getOwnSetRatingQuery);
    }

    private async Task<int> GetCountLike(Guid reviewId)
    {
        var getCountLikeQuery = new GetCountLikeQuery(reviewId);
        return await _mediator.Send(getCountLikeQuery);
    }
}