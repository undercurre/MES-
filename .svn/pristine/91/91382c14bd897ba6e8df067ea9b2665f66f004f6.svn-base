using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.Repository.Oracle.Barcode
{
	public class VendorBarcode
	{
		//public static void KeepVendorBarcode(Reel reel, string userName)
		//{
		//	bool reelExists = ReelManager.ReelExists(reel.CODE);
		//	if (reelExists) return;

		//	String CMD_GETSEQUENCE = "SELECT IMS_REEL_SEQ.NEXTVAL@WMS FROM DUAL";
		//	reel.ID = EZ.DB.DBA.FromDb("MES").ExecuteScalar<decimal>(CMD_GETSEQUENCE);
	
		//	GetVendorId(reel);
		//	GetPartId(reel);
		//	ReelDataSet.IMS_REELDataTable reelTable = new ReelDataSet.IMS_REELDataTable();
		//	ReelDataSet.IMS_REELRow reelRow = null;

		//	reelRow = reelTable.NewIMS_REELRow();
		//	reelRow.BeginEdit();
		//	reelRow.ID = reel.ID;
		//	reelRow.VERSION = 1;
		//	reelRow.CODE = reel.CODE;
		//	reelRow.BOX_ID = -1;
		//	reelRow.VENDOR_ID = reel.VendorID;
		//	reelRow.PART_ID = reel.PartID;
		//	reelRow.DATE_CODE = reel.DateCode;
		//	reelRow.LOT_CODE = reel.LotCode.IsNullOrEmpty() ? reel.DateCode.ToString() : reel.LotCode;
		//	reelRow.CASE_QTY = reel.CaseQty <= 0 ? reel.Quantity : reel.CaseQty; //获取p2最小包装数量
		//	reelRow.ORIGINAL_QUANTITY = reel.Quantity;
		//	reelRow.CUSTOMER_PN = reel.CustomerPN.IsNullOrEmpty() ? "" : reel.CustomerPN;
		//	reelRow.REFERENCE = reel.REF.IsNullOrEmpty() ? "" : reel.REF;

		//	string msdLevel = "1"; //获取MSD等级
		//	reelRow.MSD_LEVEL = msdLevel;
		//	reelRow.ESD_FLAG = GlobalVariables.EnableN; //获取ESD标记
		//												//获取料号描述
		//	WareHouseBasisDataSet.IMS_PARTRow partRow = PartManager.GetIMSPartTable(
		//		new KeyValuePair<string, object>(GlobalVariables.ID, reel.PartID)
		//			).FirstRow().Cast<WareHouseBasisDataSet.IMS_PARTRow>();

		//	reelRow.DESCRIPTION = partRow.IsDESCRIPTIONNull() ? string.Empty : partRow.DESCRIPTION;
		//	reelRow.TO_LOCATOR_ID = -1;
		//	reelRow.ORIGINAL_SIC_ID = -1;
		//	reelRow.MAKER_PART_CODE = reel.MakerPN;
		//	//reelRow.CREATED_BY = userName;
		//	//reelRow.CREATED_DATE = serverDate;
		//	//reelRow.LAST_UPDATE_BY = userName;
		//	//reelRow.LAST_UPDATE_DATE = serverDate;

		//	reelRow.EndEdit();
		//	reelTable.AddIMS_REELRow(reelRow);

		//	EZ.DB.DBA.FromDb("MES").ExecuteProcedureByKeyValuePairs("PRO_INSERT_IMS_REEL",
		//		new KeyValuePair<string, object>("P_ID", reelRow.ID),
		//		new KeyValuePair<string, object>("P_VERSION", reelRow.VERSION),
		//		new KeyValuePair<string, object>("P_CODE", reelRow.CODE),
		//		new KeyValuePair<string, object>("P_BOX_ID", reelRow.BOX_ID),
		//		new KeyValuePair<string, object>("P_VENDOR_ID", reelRow.VENDOR_ID),
		//		new KeyValuePair<string, object>("P_PART_ID", reelRow.PART_ID),
		//		new KeyValuePair<string, object>("P_DATE_CODE", reelRow.DATE_CODE),
		//		new KeyValuePair<string, object>("P_LOT_CODE", reelRow.LOT_CODE),
		//		new KeyValuePair<string, object>("P_CASE_QTY", reelRow.CASE_QTY),
		//		new KeyValuePair<string, object>("P_ORIGINAL_QUANTITY", reelRow.ORIGINAL_QUANTITY),
		//		new KeyValuePair<string, object>("P_CUSTOMER_PN", reelRow.CUSTOMER_PN),
		//		new KeyValuePair<string, object>("P_REFERENCE", reelRow.REFERENCE),
		//		new KeyValuePair<string, object>("P_MSD_LEVEL", reelRow.MSD_LEVEL),
		//		new KeyValuePair<string, object>("P_ESD_FLAG", reelRow.ESD_FLAG),
		//		new KeyValuePair<string, object>("P_DESCRIPTION", reelRow.DESCRIPTION),
		//		new KeyValuePair<string, object>("P_TO_LOCATOR_ID", reelRow.TO_LOCATOR_ID),
		//		new KeyValuePair<string, object>("P_ORIGINAL_SIC_ID", reelRow.ORIGINAL_SIC_ID));
		//}
	}
}
