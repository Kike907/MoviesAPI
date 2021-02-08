using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTO
{
    public class UserToken
    {
        public string Token { get; set; }
        
        public DateTime Expiration { get; set; }
    }
}