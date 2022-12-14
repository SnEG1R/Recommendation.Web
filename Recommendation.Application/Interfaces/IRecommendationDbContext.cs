﻿using Microsoft.EntityFrameworkCore;
using Recommendation.Domain;

namespace Recommendation.Application.Interfaces;

public interface IRecommendationDbContext
{
    DbSet<UserApp> Users { get; set; }
    DbSet<Review> Reviews { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Tag> Tags { get; set; }
    DbSet<Rating> Ratings { get; set; }
    DbSet<Like> Likes { get; set; }
    DbSet<Comment> Comments { get; set; }
    DbSet<Composition> Compositions { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();
}