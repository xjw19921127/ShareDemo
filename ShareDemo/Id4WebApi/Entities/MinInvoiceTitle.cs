//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具 FreeSql.Generator 生成。
//     运行时版本:3.1.1
//     Website: https://github.com/2881099/FreeSql
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
namespace Id4WebApi
{

	[JsonObject(MemberSerialization.OptIn), Table(Name = "min_invoice_title")]
	public partial class MinInvoiceTitle {

		/// <summary>
		/// 银行账号
		/// </summary>
		[JsonProperty, Column(Name = "bank_account", DbType = "varchar(50)")]
		public string BankAccount { get; set; } = string.Empty;

		/// <summary>
		/// 银行名称
		/// </summary>
		[JsonProperty, Column(Name = "bank_name", DbType = "varchar(100)")]
		public string BankName { get; set; } = string.Empty;

		/// <summary>
		/// 单位地址
		/// </summary>
		[JsonProperty, Column(Name = "company_address", DbType = "varchar(200)")]
		public string CompanyAddress { get; set; } = string.Empty;

		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty, Column(Name = "create_time", DbType = "datetime")]
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		[JsonProperty, Column(Name = "create_user", DbType = "varchar(20)")]
		public string CreateUser { get; set; } = string.Empty;

		/// <summary>
		/// 数据有效性：0无效，1有效
		/// </summary>
		[JsonProperty, Column(Name = "data_flag")]
		public int DataFlag { get; set; }

		/// <summary>
		/// 主键
		/// </summary>
		[JsonProperty, Column(Name = "identity_id", IsIdentity = true)]
		public long IdentityId { get; set; }

		/// <summary>
		/// 发票详情Guid
		/// </summary>
		[JsonProperty, Column(Name = "invoice_detail_id", DbType = "varchar(36)")]
		public string InvoiceDetailId { get; set; } = string.Empty;

		/// <summary>
		/// 是否删除：0未删除，1已删除
		/// </summary>
		[JsonProperty, Column(Name = "is_deleted")]
		public int IsDeleted { get; set; }

		/// <summary>
		/// 手机号码
		/// </summary>
		[JsonProperty, Column(Name = "mobile", DbType = "varchar(20)")]
		public string Mobile { get; set; } = string.Empty;

		/// <summary>
		/// 发票抬头名称
		/// </summary>
		[JsonProperty, Column(Name = "name", DbType = "varchar(50)")]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// GUID主键
		/// </summary>
		[JsonProperty, Column(Name = "pid", DbType = "varchar(36)")]
		public string Pid { get; set; } = string.Empty;

		/// <summary>
		/// 备注
		/// </summary>
		[JsonProperty, Column(Name = "remark", DbType = "varchar(200)")]
		public string Remark { get; set; } = string.Empty;

		/// <summary>
		/// 税号
		/// </summary>
		[JsonProperty, Column(Name = "tax_number", DbType = "varchar(20)")]
		public string TaxNumber { get; set; } = string.Empty;

		/// <summary>
		/// 发票抬头类型：0单位，1个人
		/// </summary>
		[JsonProperty, Column(Name = "type")]
		public int Type { get; set; }

		/// <summary>
		/// 更新时间
		/// </summary>
		[JsonProperty, Column(Name = "update_time", DbType = "datetime")]
		public DateTime UpdateTime { get; set; }

		/// <summary>
		/// 更新人
		/// </summary>
		[JsonProperty, Column(Name = "update_user", DbType = "varchar(20)")]
		public string UpdateUser { get; set; } = string.Empty;

		/// <summary>
		/// 版本号
		/// </summary>
		[JsonProperty, Column(Name = "version")]
		public long Version { get; set; }

	}

}
