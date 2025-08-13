using System;
using System.Collections.Generic;
using BountyHuntersBlog.Data.Models;

namespace BountyHuntersBlog.UnitTests.TestData
{
    public static class SampleData
    {
        public static List<Mission> Missions() => new()
        {
            new Mission
            {
                Id = 1,
                Title = "Alpha",
                Description = "Desc A",
                ImageUrl = null,
                CategoryId = 10,
                Category = new Category { Id = 10, Name = "Ops" },
                IsCompleted = false,
                CreatedOn = DateTime.UtcNow.AddDays(-2),
                MissionTags = new List<MissionTag>
                {
                    new MissionTag
                    {
                        MissionId = 1,
                        TagId = 100,
                        Tag = new Tag { Id = 100, Name = "Stealth" }
                    }
                },
                Likes = new List<Like>(),
                Comments = new List<Comment>()
            },
            new Mission
            {
                Id = 2,
                Title = "Bravo",
                Description = "Desc B",
                ImageUrl = null,
                CategoryId = 20,
                Category = new Category { Id = 20, Name = "Recon" },
                IsCompleted = true,
                CreatedOn = DateTime.UtcNow.AddDays(-1),
                MissionTags = new List<MissionTag>
                {
                    new MissionTag
                    {
                        MissionId = 2,
                        TagId = 200,
                        Tag = new Tag { Id = 200, Name = "HighRisk" }
                    }
                },
                Likes = new List<Like>(),
                Comments = new List<Comment>
                {
                    new Comment { Id = 5, MissionId = 2, UserId = "user-2", Content = "hi", CreatedOn = DateTime.UtcNow }
                }
            }
        };
    }
}
