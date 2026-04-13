using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Models.Models
{
    public class UsersModel
    {
        public class BrowseModel
        {
            public class Request
            {
                public int? UserId { get; set; }
            }
            public class Return : ReturnModel
            {
                public List<ReturnData?> Data { get; set; }
            }
            public class ReturnData
            {
                public int? UserId { get; set; }
                public string? UserName { get; set; }
                public string? Email { get; set; }
                public int? RoleId { get; set; }
                public DateTime? CreatedDate { get; set; }
                public DateTime? UpdatedDate { get; set; }
            }
        }
        public class CreateModel
        {
            public class Request
            {
                public string? Email { get; set; }
                public string? UserName { get; set; }
                public int? RoleId { get; set; }
            }
            public class Return : ReturnModel
            {
                public List<ReturnData?> Data { get; set; }
            }
            public class ReturnData
            {
                public int? UserId { get; set; }
                public string? UserName { get; set; }
                public string? Email { get; set; }
                public int? RoleId { get; set; }
                public DateTime? CreatedDate { get; set; }
                public DateTime? UpdatedDate { get; set; }
            }
        }
        public class UpdateModel
        {
            public class Request
            {
                public int? UserId { get; set; }
                public string? UserName { get; set; }
                public string? Email { get; set; }
                public int? RoleId { get; set; }
            }
            public class Return : ReturnModel
            {
                public List<ReturnData?> Data { get; set; }
            }
            public class ReturnData
            {
                public int? UserId { get; set; }
                public string? UserName { get; set; }
                public string? Email { get; set; }
                public int? RoleId { get; set; }
                public DateTime? CreatedDate { get; set; }
                public DateTime? UpdatedDate { get; set; }
            }
        }
        public class DeleteModel
        {
            public class Request
            {
                public int? UserId { get; set; }
            }
            public class Return : ReturnModel
            {
                public List<ReturnData?> Data { get; set; }
            }
            public class ReturnData
            {
                public int? UserId { get; set; }
                public string? UserName { get; set; }
                public string? Email { get; set; }
                public int? RoleId { get; set; }
                public DateTime? CreatedDate { get; set; }
                public DateTime? UpdatedDate { get; set; }
            }
        }
    }
}



