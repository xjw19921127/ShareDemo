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
namespace WebApi
{

	[JsonObject(MemberSerialization.OptIn), Table(Name = "min_service_order")]
	public partial class MinServiceOrder {

		/// <summary>
		/// 预约时间
		/// </summary>
		[JsonProperty, Column(Name = "appointment_time", DbType = "datetime")]
		public DateTime AppointmentTime { get; set; }

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
		/// 客需详情Json
		/// </summary>
		[JsonProperty, Column(Name = "detail", DbType = "text")]
		public string Detail { get; set; } = string.Empty;

		/// <summary>
		/// 主键
		/// </summary>
		[JsonProperty, Column(Name = "identity_id", IsIdentity = true)]
		public long IdentityId { get; set; }

		/// <summary>
		/// 是否删除：0未删除，1已删除
		/// </summary>
		[JsonProperty, Column(Name = "is_deleted")]
		public int IsDeleted { get; set; }

		/// <summary>
		/// 手机号
		/// </summary>
		[JsonProperty, Column(Name = "mobile", DbType = "varchar(50)")]
		public string Mobile { get; set; } = string.Empty;

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
		/// 房间号
		/// </summary>
		[JsonProperty, Column(Name = "room_no", DbType = "varchar(10)")]
		public string RoomNo { get; set; } = string.Empty;

		/// <summary>
		/// 客需状态，0预约中，1受理中，2已完成，3已取消，4已超时，5已过期
		/// </summary>
		[JsonProperty, Column(Name = "status")]
		public int Status { get; set; }

		/// <summary>
		/// 门店Guid
		/// </summary>
		[JsonProperty, Column(Name = "store_id", DbType = "varchar(50)")]
		public string StoreId { get; set; } = string.Empty;

		/// <summary>
		/// 工单类型
		/// </summary>
		[JsonProperty, Column(Name = "type")]
		public int Type { get; set; }

		/// <summary>
		/// 用户Guid
		/// </summary>
		[JsonProperty, Column(Name = "uesr_id", DbType = "varchar(50)")]
		public string UesrId { get; set; } = string.Empty;

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
		/// 用户扩展Guid
		/// </summary>
		[JsonProperty, Column(Name = "user_extend_id", DbType = "varchar(50)")]
		public string UserExtendId { get; set; } = string.Empty;

		/// <summary>
		/// 版本号
		/// </summary>
		[JsonProperty, Column(Name = "version")]
		public long Version { get; set; }

	}

}
