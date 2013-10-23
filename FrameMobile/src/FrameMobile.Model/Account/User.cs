using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Account
{
    [Serializable]
    [SubSonicTableNameOverride("user")]
    public class User : MySQLModelBase
    {
        /// <summary>
        /// 用户组
        /// </summary>
        [Display(Name = "用户组Id")]
        [Required(ErrorMessage = "×")]
        [System.Web.Mvc.Remote("Exists", "Account", ErrorMessage = "用户名已存在")]
        public int UserGroupId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名", Description = "4-20个字符。")]
        [Required(ErrorMessage = "×")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "×")]
        [SubSonicStringLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [Required(ErrorMessage = "×")]
        [StringLength(512)]
        [SubSonicStringLength(64)]
        public string Password { get; set; }

        /// <summary>
        /// 性别【0-男；1-女；2-保密】
        /// </summary>
        [Display(Name = "性别")]
        [Required(ErrorMessage = "×")]
        [Range(0, 2, ErrorMessage = "×")]
        public byte Gender { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Display(Name = "Email", Description = "请输入您常用的Email。")]
        [Required(ErrorMessage = "×")]
        //[EmailAddress(ErrorMessage = "×")]
        [SubSonicStringLength(128)]
        public string Email { get; set; }

        /// <summary>
        /// 密保问题
        /// </summary>
        [Display(Name = "密保问题", Description = "请正确填写，在您忘记密码时用户找回密码。4-20个字符。")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "×")]
        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string SecurityQuestion { get; set; }

        /// <summary>
        /// 密保答案
        /// </summary>
        [Display(Name = "密保答案", Description = "请认真填写，忘记密码后回答正确才能找回密码。2-20个字符。")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "×")]
        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string SecurityAnswer { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>
        [Display(Name = "QQ号码")]
        [RegularExpression("^[1-9][0-9]{4,13}$", ErrorMessage = "×")]
        [StringLength(12, MinimumLength = 5, ErrorMessage = "×")]
        [SubSonicNullString]
        public string QQ { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        [Display(Name = "电话号码", Description = "常用的联系电话（手机或固话），固话格式为：区号-号码。")]
        [RegularExpression("(\\(\\d{3,4}\\)|\\d{3,4}-|\\s)?\\d{7,14}", ErrorMessage = "×")]
        [SubSonicNullString]
        public string Tel { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [Display(Name = "联系地址", Description = "常用地址，最多80个字符。")]
        [StringLength(80, ErrorMessage = "×")]
        [SubSonicNullString]
        public string Address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [Display(Name = "邮编")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "×")]
        [SubSonicNullString]
        public string PostCode { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 上次修改时间
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string Comment { get; set; }
    }
}
