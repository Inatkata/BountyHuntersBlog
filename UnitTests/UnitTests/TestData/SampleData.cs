using System;
using System.Collections.Generic;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.UnitTests.TestData
{
    public static class SampleData
    {
        public static List<Category> GetCategories() => new()
        {
            new Category { Id = 1, Name = "Bounties", CreatedOn = DateTime.UtcNow },
            new Category { Id = 2, Name = "Hunting", CreatedOn = DateTime.UtcNow }
        };

        public static List<Tag> GetTags() => new()
        {
            new Tag { Id = 1, Name = "Stealth", CreatedOn = DateTime.UtcNow },
            new Tag { Id = 2, Name = "Sharpshooter", CreatedOn = DateTime.UtcNow }
        };

        public static List<Mission> GetMissions() => new()
        {
            new Mission
            {
                Id = 1,
                Title = "Catch the outlaw",
                ImageUrl = "https://img/outlaw.jpg",
                CategoryId = 1,
                CreatedOn = DateTime.UtcNow
            },
            new Mission
            {
                Id = 2,
                Title = "Defend the village",
                ImageUrl = "https://img/village.jpg",
                CategoryId = 2,
                CreatedOn = DateTime.UtcNow
            }
        };

        public static List<Comment> GetComments() => new()
        {
            new Comment
            {
                Id = 1,
                Content = "Great mission!",
                MissionId = 1,
                UserId = "user1",
                CreatedOn = DateTime.UtcNow
            },
            new Comment
            {
                Id = 2,
                Content = "Needs more detail",
                MissionId = 2,
                UserId = "user2",
                CreatedOn = DateTime.UtcNow
            }
        };
    }
}