using AutoMapper;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZ.IMS.Admin.Profiles
{
	public class MapperProfiles : Profile
	{
		public MapperProfiles()
		{
			#region ManagerRole

			CreateMap<ManagerRoleAddOrModifyModel, Sys_Manager_Role>();

			CreateMap<Sys_Manager_Role, ManagerRoleListModel>();

			#endregion

			#region Manager

			CreateMap<Sys_Manager, ManagerListModel>();

			CreateMap<ManagerAddOrModifyModel, Sys_Manager>();

			CreateMap<ChangeInfoModel, Sys_Manager>();

			#endregion

			#region Menu

			CreateMap<MenuAddOrModifyModel, Sys_Menu>();
			CreateMap<Sys_Menu, MenuNavView>();

			#endregion

			#region 点检

			CreateMap<SfcsEquipKeepHead, EquipKeepHeadModel>();
			//.ForMember(dest => dest.KEEP_CHECK_STATUS, opt => opt.Ignore());

			#endregion

			#region 产品解锁

			CreateMap<SfcsHoldProductHeader, SfcsHoldProductHeaderListModel>();

			#endregion

			#region 产前确认

			CreateMap<MesProductionPreConf, MesProductionPreConfListModel>();

			CreateMap<MesProductionPreMst, MesProductionPreMstListModel>();

			#endregion

			#region 组织架构

			CreateMap<SysOrganize, SysOrganizeListModel>();

			#endregion

			#region 首件管理

			CreateMap<MesQualityItems, MesQualityItemsAddOrModifyModel>();
			CreateMap<MesQualityItemsAddOrModifyModel,MesQualityItems>();

			#endregion

			#region 镭雕机
			CreateMap<SfcsRuncardRanger, SfcsLaserRangerAddOrModifyModel>();

			#endregion
		}
	}
}
