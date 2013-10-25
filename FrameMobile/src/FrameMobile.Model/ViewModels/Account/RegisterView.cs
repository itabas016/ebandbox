using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FrameMobile.Model.Account;

namespace FrameMobile.Model
{
    public class RegisterView : User
    {
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码", Description = "6-20个字符。")]
        [Required(ErrorMessage = "×")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "×")]
        [DataType(DataType.Password)]
        public new string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Display(Name = "确认密码", Description = "再次输入密码。")]
        [Compare("Password", ErrorMessage = "×")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Display(Name = "验证码", Description = "请输入图片中的验证码。")]
        [Required(ErrorMessage = "×")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "×")]
        public string VerificationCode { get; set; }

        /// <summary>
        /// 邀请码
        /// </summary>
        [Display(Name = "邀请码", Description = "请输入你的邀请码，区分大小写，且只能用一次。")]
        [Required(ErrorMessage = "×")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "×")]
        public string InvitationCode { get; set; }
    }
}
