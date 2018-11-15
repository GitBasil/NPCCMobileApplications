using Android.Graphics;
using SQLite;

namespace NPCCMobileApplications.Library
{
    [Table("Employee")]
    public class Spools
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Ename { get; set; }
        public string Job { get; set; }
        public decimal Salary { get; set; }
        public string icon { get; set; }
    }
}
