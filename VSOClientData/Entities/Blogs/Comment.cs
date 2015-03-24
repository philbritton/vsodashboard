using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace VSOClientData.Entities.Blogs
{
    public class Comment : MongoEntity
    {
        [ScaffoldColumn(false)]
        public int CommentId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime Date { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
