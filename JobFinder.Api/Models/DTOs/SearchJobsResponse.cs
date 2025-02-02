﻿using JobFinder.Api.Models.ValueObjects;
using System.Text.Json.Serialization;

namespace JobFinder.Api.Models.DTOs
{
    public class SearchJobsResponse
    {
        [JsonPropertyName("numberOfHits")]
        public int NumberOfHits { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("jobs")]
        public List<Job> Jobs { get; set; }
    }
}
