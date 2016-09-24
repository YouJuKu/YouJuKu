using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DHTMLX.Scheduler;
 
namespace YouJuku.Models
{
    public class SchedulerEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DHXJson(Alias = "id")]
        public int ID { get; set; }
 
        [DHXJson(Alias = "text")]
        public string Text { get; set; }
 
        [DHXJson(Alias = "start_date")]
        public DateTime StartDate { get; set; }
 
        [DHXJson(Alias = "end_date")]
        public DateTime EndDate { get; set; }
 
        public string UserId { get; set; }
    }
}