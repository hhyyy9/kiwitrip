using System;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Identity;

namespace back_end.Entities
{
    [DynamoDBTable("User")]
    public class User
    {
        public User() {}

        [DynamoDBHashKey("id")]
        public string Id { get; set; }
        [DynamoDBProperty("username")]
        public string Username { get; set; }
        [DynamoDBProperty("password")]
        public string Password { get; set; }
    }
}

