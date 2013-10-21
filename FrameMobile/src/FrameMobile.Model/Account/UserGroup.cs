using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Account
{
    [Serializable]
    [SubSonicTableNameOverride("usergroup")]
    public class UserGroup : MySQLModelBase
    {
        /// <summary>
        /// 用户组名称
        /// </summary>
        [Display(Name = "名称", Description = "2-12个字符。")]
        [Required(ErrorMessage = "×")]
        [StringLength(12, MinimumLength = 2, ErrorMessage = "×")]
        public string Name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Display(Name = "说明", Description = "最多50个字符。")]
        [StringLength(50, ErrorMessage = "×")]
        public string Description { get; set; }
    }
}
