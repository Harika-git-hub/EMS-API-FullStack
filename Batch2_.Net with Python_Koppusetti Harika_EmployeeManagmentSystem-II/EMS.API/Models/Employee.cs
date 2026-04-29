using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.API.Models
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Column("FirstName")]
        public string? FirstName { get; set; }

        [Column("LastName")]
        public string? LastName { get; set; }

        [Column("Email")]
        public string? Email { get; set; }

        [Column("Phone")]
        public string? Phone { get; set; }

        [Column("Department")]
        public string? Department { get; set; }

        [Column("Designation")]
        public string? Designation { get; set; }

        [Column("Salary", TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        [Column("JoinDate")]
        public DateTime? JoinDate { get; set; }

        [Column("Status")]
        public string? Status { get; set; }

        [Column("CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        [Column("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; } // Add this
    }
}
