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
    public class UserGroup : MySQLModel
    {
        /// <summary>
        /// 用户组名称
        /// </summary>
        [Display(Name = "名称", Description = "2-20个字符。")]
        [Required(ErrorMessage = "×")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "×")]
        public override string Name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Display(Name = "说明", Description = "最多100个字符。")]
        [StringLength(100, ErrorMessage = "×")]
        public string Description { get; set; }
    }
}
