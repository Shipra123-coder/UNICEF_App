using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Entities
{
    [Table("m_UserLevel")]
    public class m_UserLevel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

    //public class UserLevelViewModel
    //{
    //    public m_UserLevel FormModel { get; set; } = new m_UserLevel();
    //    public IEnumerable<m_UserLevel> UserLevelList { get; set; } = new List<m_UserLevel>();
    //}
}

