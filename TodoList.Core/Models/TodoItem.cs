using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList.Core.Models
{
    public class TodoItem
    {
        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(450)]
        public string UserId { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(200)]
        [MinLength(15)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public bool Done { get; set; }
        [MaxLength(500)]
        public string ImagePath { get; set; }
        [DataType(DataType.DateTime)]
        [Column("Added")]
        public DateTime AddedDateTime { get; set; }
        [DataType(DataType.DateTime)]
        [Column("DueTo")]
        public DateTime DueToDateTime { get; set; }

    }
}