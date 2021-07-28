using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }
    }
}