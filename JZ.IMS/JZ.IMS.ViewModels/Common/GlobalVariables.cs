using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JZ.IMS.ViewModels
{
    public static class GlobalVariables
    {
        public const int successCode = 1;
        public const int FailedCode = 0;
        //public static string SmtSchemaName = Globals.SchemaName;
        //radix
        public const string HexRadix = "0123456789ABCDEF";
        public const string DecRadix = "0123456789";
        public const string Base36Redix = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string Base33Redix = "0123456789ABCDEFGHJKLMNPRSTUVWXYZ";
        public const string IntelServerCarton34Redix = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
        public const string OnlyWordsString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string SmtConnecName = "MES";
        public const string Permission = "Permission";
        public static decimal PrintB = 881;
        public static decimal PrintT = 884;

        public const decimal PCB_TOP = 1;
        public const decimal PCB_BOTTOM = 2;
        public const decimal PCB_TOP_AND_BOTTOM = 3;

        public const string EnableY = "Y";
        public const string EnableN = "N";
        public const string Enabled = "ENABLED";
        public const string Checked = "CHECKED";
        public const int FailNUM = -1;//返回受影响的行

        public const char comma = ',';
        public const char semicolon = ';';
        public const char tilde = '~';
        public const char leftParenthesis = '(';
        public const char rightParenthesis = ')';
        public const char dot = '.';
        public const string Equal = "=";
        public const string Hyphen = "-";
        public const string Colon = ":";
        public const string splitChar = @"\";
        public const char VerticalChar = '|';
        public const string Space = " ";
        public const string NA = "N/A";
        public const string NULL = "NULL";
        public const string EnterMark = "\n";
        public const string PPIDPrefix = "$PPID";
        public const string ASN2dSplit = "$%";
        public const string AddSpliterChar = "+";

        #region Column name

        public const string FEEDER = "FEEDER";
        public const string ID = "ID";
        public const string VERSION = "VERSION";
        public const string EMPNO = "EMPNO";
        public const string PART_NO = "PART_NO";
        public const string WO_NO = "WO_NO";
        public const string PARENT_ID = "PARENT_ID";
        public const string MODEL = "MODEL";
        public const string WORK_TIME = "WORK_TIME";
        public const string STATUS = "STATUS";
        public const string TYPE = "TYPE";
        public const string CODE = "CODE";
        public const string VALUE = "VALUE";
        public const string CN_DESC = "CN_DESC";
        public const string EN_DESC = "EN_DESC";
        public const string LOOKUP_CODE = "LOOKUP_CODE";
        public const string MEANING = "MEANING";
        public const string LINE_ID = "LINE_ID";
        public const string LINE_NAME = "LINE_NAME";
        public const string SMT_LINE = "SMT_LINE";
        public const string CN = "CN";
        public const string SUBINVENTORY = "SUBINVENTORY";
        public const string BATCH_NO = "BATCH_NO";
        public const string TRANSACTION_NO = "TRANSACTION_NO";
        public const string MST_ID = "MST_ID";
        public const string REEL_ID = "REEL_ID";
        public const string MAKER_ID = "MAKER_ID";
        public const string MAKER_PART_ID = "MAKER_PART_ID";
        public const string STOCK_ID = "STOCK_ID";
        public const string COMPANY_ID = "COMPANY_ID";
        public const string INTERFACE_MST_ID = "INTERFACE_MST_ID";
        public const string INTERFACE_DTL_ID = "INTERFACE_DTL_ID";
        public const string PO_DTL_ID = "PO_DTL_ID";
        public const string SLOT_NO = "SLOT_NO";
        public const string COMPONENT_PN = "COMPONENT_PN";
        public const string COMPONENT_SN = "COMPONENT_SN";
        public const string ADDON_CLASS_TYPE = "ADDON_CLASS_TYPE";
        public const string ADDON_ID = "ADDON_ID";
        public const string DELETE_FLAG = "DELETE_FLAG";
        public const string IS_NPI_WO = "IS_NPI_WO";
        public const string BOX_ID = "BOX_ID";
        public const string VENDOR_CODE = "VENDOR_CODE";
        public const string PART_DESC = "PART_DESC";
        public const string MSD_LEVEL = "MSD_LEVEL";
        public const string MAKER_NAME = "MAKER_NAME";
        public const string MAKER_PART_NO = "MAKER_PART_NO";
        public const string QUANTITY = "QUANTITY";
        public const string CUSTOMER_PN = "CUSTOMER_PN";
        public const string COO = "COO";
        public const string LOT_CODE = "LOT_CODE";
        public const string DATE_CODE = "DATE_CODE";
        public const string CASE_QTY = "CASE_QTY";
        public const string ITF_STOCK_ID = "ITF_STOCK_ID";
        public const string EDI_FLAG = "EDI_FLAG";
        public const string PLACEMENT = "PLACEMENT";
        public const string SMT_NAME = "SMT_NAME";
        public const string STATION_ID = "STATION_ID";
        public const string ORDER_NO = "ORDER_NO";
        public const string KIND = "KIND";
        public const string CONFIG_TYPE = "CONFIG_TYPE";
        public const string PO_CODE = "PO_CODE";
        public const string MSL_PO_NO = "MSL_PO_NO";
        public const string CUSTOMER_PO_NO = "CUSTOMER_PO_NO";
        public const string PCB_SIDE = "PCB_SIDE";
        public const string FEEDER_ID = "FEEDER_ID";
        public const string PRODUCT_OPERATION_CODE = "PRODUCT_OPERATION_CODE";

        //IMS_LOCATOR
        public const string NAME = "NAME";
        public const string DESCRIPTION = "DESCRIPTION";

        //IMS_IQC_PARTS
        public const string BU_ID = "BU_ID";
        public const string VENDOR_ID = "VENDOR_ID";
        public const string PART_ID = "PART_ID";

        //IMS_AUTODD_INFO
        public const string SIC_ID = "SIC_ID";
        public const string LOCATOR_ID = "LOCATOR_ID";
        public const string TO_LOCATOR_ID = "TO_LOCATOR_ID";
        public const string FACTORY_CODE = "FACTORY_CODE";

        //IMS_PART_ATTRIBUTES
        public const string CUSTOMER = "CUSTOMER";

        //IMS_CHEM_PARTS_LOC
        public const string LOCATOR_CODE = "LOCATOR_CODE";

        //IMS_PALLET_DTL
        public const string PALLET_NO = "PALLET_NO";

        //LOC_MST
        public const string LOC_NO = "LOC_NO";
        public const string LOC_STS = "LOC_STS";
        public const string AISLE_NO = "AISLE_NO";
        public const string LOC_SIZE = "LOC_SIZE";

        //LOC_DTL
        public const string PLT_ID = "PLT_ID";
        public const string BCD_ID = "BCD_ID";

        //IMS_STOCK
        public const string REEL_CODE = "REEL_CODE";

        // IQC
        public const string IQC_REMARK = "IQC_REMARK";

        // 
        public const string PRINT_CODE_NAME = "PRINT_CODE_NAME";

        // SIC
        public const string SIC_CODE = "SIC_CODE";
        public const string BU_NAME = "BU_NAME";

        // 線邊倉備料
        public const string MERGE_SLOT_ID = "MERGE_SLOT_ID";

        #endregion

        //SMT_REEL Status
        public const decimal Smt_Reel_Status_Free = 0;
        public const decimal Smt_Reel_Status_Supply = 2;
        public const decimal Smt_Reel_Status_Place = 3;
        public const decimal Smt_Reel_Status_Closed = 4;
        public const decimal Smt_Reel_Status_Backup = 5;
        public const decimal Smt_Reel_Status_Connected = 6;

        //SMT Copy
        public static decimal CopyType = 0;
        public const decimal StationConfigCopy = 1;
        public const decimal LineConfigCopy = 2;
        public const decimal RouteCopy = 3;

        //SMT
        public static decimal SMT_Line_ID = 0;
        public static int StationCount = 0;

        //pallet status
        public const decimal PalletInUsingStatus = 0;
        public const decimal PalletFinishedStatus = 1;

        //Assembly
        public static decimal Assembly_Line_ID = 0;
        public static decimal AssemblySFCS_Site_ID = 0;
        public static decimal AssemblySFCS_Route_Code = 0;
        public static string AssemblySFCS_Site_Name = string.Empty;
        public static string AssemblySFCS_Route_Name = string.Empty;

        //Pressfit
        public static decimal Pressfit_Line_ID = 0;
        public static decimal PressfitSFCS_Site_ID = 0;
        public static decimal PressfitSFCS_Route_Code = 0;
        public static string PressfitSFCS_Site_Name = string.Empty;
        public static string PressfitSFCS_Route_Name = string.Empty;

        //ICT
        public static decimal ICT_Line_ID = 0;
        public static decimal ICTSFCS_Site_ID = 0;
        public static decimal ICTSFCS_Route_Code = 0;
        public static string ICTSFCS_Site_Name = string.Empty;
        public static string ICTSFCS_Route_Name = string.Empty;

        public static decimal FEEDER_MUST_SCRAP_COUNT = 10;

        public const string PTSRegeditFolder = "JZ.SMT.PTS\\CONFIG";
        public const string SMT_Line_ID_Key = "SMT_Line_ID";

        public const string Assembly_Line_ID_KEY = "Assembly_Line_ID";
        public const string Assembly_SFCS_Site_ID_Key = "SFCS_Site_ID";
        public const string Assembly_SFCS_Route_Code_Key = "SFCS_Route_Code";
        public const string Assembly_SFCS_Site_Name_Key = "SFCS_Site_Name";
        public const string Assembly_SFCS_Route_Name_Key = "SFCS_Route_Name";

        public const string Pressfit_Line_ID_KEY = "Pressfit_Line_ID";
        public const string Pressfit_SFCS_Site_ID_Key = "PressfitSFCS_Site_ID";
        public const string Pressfit_SFCS_Route_Code_Key = "PressfitSFCS_Route_Code";
        public const string Pressfit_SFCS_Site_Name_Key = "PressfitSFCS_Site_Name";
        public const string Pressfit_SFCS_Route_Name_Key = "PressfitSFCS_Route_Name";

        public const string ICT_Line_ID_KEY = "ICT_Line_ID";
        public const string ICT_SFCS_Site_ID_Key = "ICTSFCS_Site_ID";
        public const string ICT_SFCS_Route_Code_Key = "ICTSFCS_Route_Code";
        public const string ICT_SFCS_Site_Name_Key = "ICTSFCS_Site_Name";
        public const string ICT_SFCS_Route_Name_Key = "ICTSFCS_Route_Name";

        #region SMT LOOKUP TYPE

        public const string SensorJudge = "SENSOR_SIGNAL";
        public const int ContinueSignalTime = 1000;
        public const string K_Cycle_Stop_String = "<K`,,255><L3><L3><L3>";
        public const string K_Stop_Conway = "<L3>";
        public const string K_Restart_Conway = "<L2>";
        public const string K_Scanner_Status = "<?>";
        public const string K_Hour_Since_Last_Reset = "<K@?>";
        public const string K_NOREAD = "NOREAD";
        public const string K_NORD = "NORD";
        public const string MACHINECONFIG = "MACHINECONFIG";
        public const string LINECONFIG = "LINECONFIG";

        #endregion

        public const string FEEDER_REPAIR_RESULT_WAITTING = "WaitRepair";
        public const string FEEDER_REPAIR_RESULT_OK = "OK";
        public const string FEEDER_REPAIR_RESULT_NO = "NO";
        public const string FEEDER_REPAIR_RESULT_SCRAP = "SCRAP";
        public const string SMT_FEEDER_STATUS = "SMT_FEEDER_STATUS";
        //ranger status
        public const decimal RangerInputStatus = 2;
        public const decimal RangerNotInputStatus = 1;
        //wo status
        public const decimal WorkOrderNotInputStatus = 1;
        public const decimal WorkOrderInputStatus = 2;
        public const decimal WorkOrderClosedStatus = 3;
        public const decimal WorkOrderCommitdStatus = 4;

        //完工入库
        public const string InBoundUntreatedStatus = "0";
        public const string InBoundProcessingStatus = "1";
        public const string  InBoundProcessedStatus = "2";

        //carton status
        public const decimal CartonInUsingStatus = 0;
        public const decimal CartonFinishedStatus = 1;
        // StopLine Maintain Status
        public const decimal Open = 0;
        public const decimal InitialClosed = 1;
        public const decimal Closed = 2;
        public const decimal StopLine_RC = 3;
        public const decimal StopLine_CA = 4;

        #region Label Type
        public const decimal CartonLable = 7;
        public const decimal QcLable = 6;
        public const decimal PallectLabel = 5;

        #endregion

        #region various data type code

        // VARIOUS_DATA_TYPE
        public const decimal Root_Cause_Category = 1;
        public const decimal REPAIR_RESPONSER = 2;
        public const decimal PRODUCT_QUALITY_LOCATION = 4;
        public const decimal OQA_PRODUCT_QUALITY_AQL = 5;
        public const decimal PRINTER = 7;
        public const decimal DEFECT_CATEGORY_LOOKUP = 10;
        public const decimal BAKE_TEMPERATURE = 11;
        public const decimal DISASSEMBLY_VERIFIED_CODE = 12;
        public const decimal BAKE_INTERVAL = 13;
        public const decimal ASSEMMBLY_RESULT_CODE = 14;
        public const decimal MB_VERIFIED_RESULT_CODE = 15;
        public const decimal MINIWAVE_REASON = 16;
        public const decimal MINIWAVE_INSPECT_RESULT = 17;
        public const decimal INSPECTION_SYMPTOM_CODE = 18;
        public const decimal RESPONSIBLE_VENDOR_CODE = 19;
        public const decimal REPAIR_IN_INTERVAL = 20;
        public const decimal BGA_PEPLACE_INTERVAL = 21;
        public const decimal AXI_DEFECT_CODE = 22;
        public const decimal REPAIR_ASSEMBLY_KIND = 23;
        public const decimal OQA_SPOTCHECK_SAMPLE_PLAN = 25;
        public const decimal SPECIAL_INVENTORY = 26;
        public const decimal RMA_SITE_CODE = 27;
        public const decimal RMA_TYPE_CODE = 28;
        public const decimal RMA_OWNER_CODE = 29;
        public const decimal RMA_CUSTOMER_CODE = 30;
        public const decimal RMA_FLAG_CODE = 31;
        public const decimal RMA_STATUS_CODE = 32;
        public const decimal SENDER_CONFIG = 33;
        public const decimal IntelPackingCheckQty = 35;
        public const decimal EDI_TYPE = 36;
        public const decimal SEND_MODEL = 37;
        public const decimal RMA_CUSTOMER_TEST_SITE = 38;
        public const decimal RMA_TAKEOUT_INVENTORY = 39;
        public const decimal RMA_REPAIR_TYPE = 40;
        public const decimal CVTE_MAP_CODE = 41;
        public const decimal REPAIR_TRACE_INSPECT_RESULT = 43;
        public const decimal FTP_Config_Repair = 44;
        public const decimal FTP_Config_Snapshot = 45;
        public const decimal FTP_Config_PCB_Cray = 46;
        public const decimal SN_ALLOCATE_RANGER_HEAD = 47;
        public const decimal INSPUR_PCBA_REVISON = 48;
        public const decimal BGA_VERIFIED_RESULT_CODE = 49;
        public const decimal StopLine_Initial_Closed_Mail_By_Operation = 50;
        public const decimal Hold_Appoint_Defect_On_Appoint_Operation = 51;
        public const decimal FTP_Config_BFT_Fail_AlarmServer = 52;
        public const decimal AOI_TEST_LOG_FILE_PATH = 324283;
        public const decimal FLUX_CODE_PN = 56;
        #endregion

        #region Plant

        public const string pcPlant = "PC";
        public const string pcbPlant = "PCB";
        public const decimal pcCode = 1;
        public const decimal pcbCode = 2;
        public const decimal OrganizationID = 405;
        public const decimal OrganizationIDSSU = 896;

        #endregion

        #region MSD

        public const string LEVEL_CODE = "LEVEL_CODE";
        public const string MSD_ACTION = "MSD_ACTION";
        public const string MSD_AREA = "MSD_AREA";
        public const string ACTION_CODE = "ACTION_CODE";

        #endregion

        #region LCR

        public const string LCR_KIND = "LCR_KIND";
        public const string LCR_GAUGE_TYPE = "LCR_GAUGE_TYPE";
        public const string LCR_GAUGE_FREQUENCY = "LCR_GAUGE_FREQUENCY";

        public const string LCR_L = "L";
        public const string LCR_C = "C";
        public const string LCR_R = "R";
        public const string LCR_Z = "Z";

        #endregion

        #region Format

        //date format
        public const string CommonDateFormat = "yyyyMMdd";
        public const string ShortDateFormat = "yyMMdd";
        public const string DateFormat = "yyyy-MM-dd";
        public const string YearMonth = "yyyy-MM";
        public const string FullDateFormat = "yyyy-MM-dd HH:mm:ss";
        public const string QueryLikeMark = "%";
        public const string UnderLine = "_";
        public const string DateCodeFormat = "yyyy/MM/dd";

        #endregion

        #region Printer

        //Label Printer
        public const string PRINTER_Zebra_105SL = "Zebra 105SL";
        public const string PRINTER_Zebra_S400 = "Zebra S400";
        public const string PRINTER_Zebra_110xi = "Zebra 110Xi";
        public const string PRINTER_Intermec = "Intermec";

        //Print Code Kind
        public const decimal Reel_Label = 1;
        public const decimal Smt_Box_Label = 2;
        public const decimal Non_Smt_Box_Label = 3;
        public const decimal ASN_Label = 4;
        public const decimal PL_Header = 5;
        public const decimal Segmentation_Reel_Label = 6;
        public const decimal CyclecountHeaderLabel = 7;
        public const decimal IntelShippingLabel = 8;
        public const decimal SFCS_Pallet_Label = 9;
        public const decimal Foxconn_Reel_Label = 10;

        //PRINTER_TYPE
        public const decimal Printer_Zebra_200dpi = 2;
        public const decimal Printer_Zebra_300dpi = 1;
        public const decimal Printer_Zebra_600dpi = 3;
        public const decimal Printer_Intermec = 4;

        #endregion

        #region resource route
        //resource route
        public const decimal ResourceIntoIceBox = 1;
        public const decimal ResourceOutFromIceBox = 2;
        public const decimal ResourceStir = 3;
        public const decimal ResourceOpen = 4;
        public const decimal ResourceBeUsed = 5;
        public const decimal ResourceIntoIceBoxAgain = 6;
        public const decimal ResourceBeUsedAgain = 7;
        public const decimal ResourceOffIceBox = 8;
        public const decimal ResourceOpenAndUsed = 9;
        public const decimal ResourceTurnin = 11;
        public const decimal ResourceHeatAfterStir = 14;
        public const decimal ResourceUsedOver = 10;

        public const decimal SolderPaste = 1;
        public const decimal RedGlue = 2;
        public const decimal BlackGlue = 3;

        #endregion

        #region stencil 

        //Stencil Store Status
        public const int StencilStored = 1;
        public const int StencilTaken = 2;
        public const int StencilOnline = 3;
        public const int StencilOffline = 4;
        public const int StencilScarpStore = 5;

        //Stencil Store Opeation Type
        public const string STORE_STENCIL = "网板存储";
        public const string TAKE_STENCIL = "领用网板";
        public const string RETURN_STENCIL = "归还网板";
        public const string CHANGE_LOCATION = "变更储位";
        public const string STENCIL_ONLINE = "网板上线";
        public const string STENCIL_OFFLINE = "网板下线";
        public const string STENCIL_SCRAP_STORE = "网板报废存储";
		public const string STENCIL_MAINTAIN_SCRAP = "网板维修报废";

        //Stencil Store
        public const string PCB_PART_NO = "PCB_PART_NO";
        public const string PCB_REVISION = "PCB_REVISION";
        public const string REMARK = "REMARK";
        public const string CHECK_TIME_BEGIN = "CHECK_TIME_BEGIN";
        public const string CHECK_TIME_END = "CHECK_TIME_END";
        public const string STENCIL_OTHERS = "其他";

        //Stenci Status 
        public const decimal StencilUsed = 1;
        public const decimal StencilWarnning = 2;
        public const decimal StencilMaintain = 3;
        public const decimal StencilScrapWarnning = 4;
        public const decimal StencilScrapped = 5;
        public const decimal StencilCleaned = 6;
        public const decimal StencilStopUsed = 7;


        #endregion

        #region Scraper

        public const string LScraperPrefix = "$SCL";
        public const string RScraperPrefix = "$SCR";
        public const string RScraper = "R";
        public const string FScraper = "F";

        public const string SCRAPER_NO = "SCRAPER_NO";
        public const string SCRAPER_WARN_COUNT = "SCRAPER_WARN_COUNT";
        public const string SCRAPER_MAX_COUNT = "SCRAPER_MAX_COUNT";

        #endregion

        #region resource

        //resource status
        public const decimal ResourceUsed = 1;
        public const decimal ResourceUseup = 2;
        public const decimal ResourceDisused = 3;
        public const decimal ResourceTransfer = 4;

        //Scraper Status
        public const decimal SCRAPER_USE = 1;
        public const decimal SCRAPER_CLEAN = 2;
        public const decimal SCRAPER_ONLINE = 3;
        public const decimal SCRAPER_OFFLINE = 4;
        public const decimal SCRAPER_FAIL = 5;
        public const decimal SCRAPER_TAKEN = 6;
        public const decimal SCRAPER_STORED = 7;

        #endregion

        #region ROUTE_TYPE
        public const decimal NORMAL_ROUTE = 1;
        public const decimal RMA_ROUTE = 2;
        #endregion

        #region Operation ID

        public const decimal ScrapOperation = 990;  //AutoScrap and RepairScrap
        public const decimal WipScrapOperation = 991;   //wipScrap
        public const decimal EndOperation = 999;
        public const decimal TUOperation = 550;
        public const decimal TU2Operation = 36190;
        public const decimal TurnInOperation = 717;
        public const decimal StartOperation = 100;
        public const decimal NoneRepair = 748;
        public const decimal NoRoute = 1;
        public const decimal FAOperation = 3126;
        public const decimal PAOperation = 24728;
        public const decimal RMAICTOperation = 104221;
        public const decimal PrintBOperation = 881;
        public const decimal PrintTOperation = 884;
        public const decimal PrintOperation = 107153;
        public const decimal IST = 22322;
        public const decimal TUPI = 887;
        public const decimal TUPI_Repair = 902;
        public const decimal LinkPalletOperation = 86408;
        public const decimal Packing = 200;
        public const decimal SFT = 668296;
        public const decimal General_Repair = 3073;
        public const decimal SPI_B_Repair = 1607689;
        public const decimal SPI_T_Repair = 1607691;
        public const decimal WIFI_TEST = 23196;
        public const decimal HI_PTS = 86404;
        public const decimal LASER = 3715529;

        //P-PI
        public const decimal P_PI = 890;
        public const decimal P_PI1 = 149708;
        public const decimal P_PI2 = 97388;
        public const decimal RMA_P_PI = 104226;

        //BFT operation id
        public const decimal BFT = 889;
        public const decimal BFT_Repair = 718;
        public const decimal RMA_BFT = 206244;
        public const decimal BFT_1 = 149702;
        public const decimal BFT_SAS = 806269;
        public const decimal RMA_BFT_HMU = 104223;
        public const decimal RMA_BFT_TS = 104232;
        public const decimal RMA_BFT1 = 290031;
        public const decimal BFT_2 = 927306;
        public const decimal BFT_3 = 927307;
        public const decimal BFT_4 = 927308;
        public const decimal BFTA = 1047212;

        //OQA operation id
        public const decimal OQAOffline = 21012;
        public const decimal OQAOnline = 891;
        public const decimal OQAFFT = 1155728;
        public const decimal Pre_OQA = 909180;
        public const decimal PI_InnerOQA = 17232;
        public const decimal OQAOnlineCosmetic1 = 92910;
        public const decimal OQAF_T = 112811;
        public const decimal OQAV_I = 104227;
        public const decimal OQA_FA = 31656;
        public const decimal FOQA = 131970;
        public const decimal VOQA = 131971;
        public const decimal VOQM = 111913;
        public const decimal PICosmeticOQA = 12503;
        public const decimal CoatingOQA = 1120866;

        //AOI operation id
        public const decimal AOI = 151165;
        public const decimal AOIB = 883;
        public const decimal AOIT = 885;
        public const decimal AOIB_Repair = 894;
        public const decimal AOIT_Repair = 895;
        public const decimal SAKI_AOI = 316483;
        public const decimal HI_AOI = 1792791;

        //AXI operation id
        public const decimal AXI = 886;
        public const decimal AXI1 = 402620;

        //ICT operation id 
        public const decimal RMA_ICT = 104221;
        public const decimal RMA_ICT_T_S = 104231;
        public const decimal ICT_Repair = 86410;
        public const decimal ICTOperation = 3885;
        public const decimal ICT2Operation = 97339;
        public const decimal ICT3Operation = 1000317;
        public const decimal Manual_ICT = 282086;


        //Security operation id
        public const decimal Link_Cage = 1396377;
        public const decimal Break_Cage = 1396378;
        public const decimal Link_Lock_Number = 1396379;
        public const decimal Break_Lock_Number = 1396408;

        //Send Data Operaton id 
        public const decimal SEND_DATA = 22321;
        public const decimal SEND_DATA2 = 37105;

        /// <summary>
        /// 过程检验 工序类别 QC SFCS_PARAMETERS.LOOKUP_CODE
        /// </summary>
        public const decimal QCOperation = 25;
        /// <summary>
        /// 完工检验 工序类别 OQA SFCS_PARAMETERS.LOOKUP_CODE
        /// </summary>
        public const decimal OQAOperation = 6;

        #endregion

        #region stopline

        // STOPLINE_MODE
        public const decimal StopLineInputControl = 0;
        public const decimal FailCountInBaseCountControl = 1;
        public const decimal FailCountInBaseTimeControl = 2;
        public const decimal FailRateInBaseTimeControl = 3;
        public const decimal NDFCountInBaseCountControl = 4;
        public const decimal NDFCountInBaseTimeControl = 5;
        public const decimal NDFRateInBaseTimeControl = 6;
        public const decimal ContinuousFailCountControl = 7;
        public const decimal ContinuousDefectCodeControl = 8;
        public const decimal LocationFailRateInBaseTimeControl = 9;
        public const decimal SMTComponentFailCountInBaseCountControl = 10;
        public const decimal SMTComponentLocationFailCountInBaseCountControl = 11;
        public const decimal SMTMachineFailCountInBaseCountControl = 12;
        public const decimal SMTSlotFailCountInBaseCountControl = 13;
        public const decimal SameErrorCodeInBaseCountControl = 14;

        // Unit(單位)
        public const decimal Pieces = 1;
        public const decimal Hour = 2;

        #endregion

        /// <summary>
        /// 工單
        /// </summary>
        public const string SMTClassfication = "SMT";
        public const string PCBClassfication = "PCB";
        public const string SYSClassfication = "SYS";
        public const string DUMMYClassfication = "DUMMY";
        public const decimal SMTClassficationCode = 1;
        public const decimal PCBClassficationCode = 2;
        public const decimal SYSClassficationCode = 3;
        public const decimal DUMMYClassficationCode = 4;
        public const string DummyPartNumberMark = "DP";
        public const string DummyWorkOrderMark = "DW";

        //wo type
        public const decimal NormalWO = 1;
        public const decimal ReworkWO = 2;
        public const decimal RMAWO = 3;

        #region product config type

        //Product Config Type
        public const decimal SNFormat = 1;
        public const decimal AK_PART_NO = 2;
        public const decimal AK_SN_FORMAT = 3;
        public const decimal PLATFORM_TYPE = 16;
        public const decimal ProductRevision = 18;
        public const decimal ProductName = 19;
        public const decimal ProductLocalization = 20;
        public const decimal REMOTECTRL = 22;
        public const decimal HWKIT = 23;
        public const decimal MOUSE = 24;
        public const decimal MSOFFICE = 25;
        public const decimal KEYBOARD = 26;
        public const decimal CPU_STEP = 27;
        public const decimal FRUMODEL = 28;
        public const decimal UPC_CODE = 29;
        public const decimal JAN_CODE = 30;
        public const decimal FRU_PN = 31;
        public const decimal EC_CODE = 32;
        public const decimal CHECK_PPID = 33;
        public const decimal CARTON_WEIGHT = 37;
        public const decimal BOX_WEIGHT = 38;
        public const decimal SYSTEM_AK_WEIGHT = 39;
        public const decimal CONTAINS_LEAD = 40;
        public const decimal RelationModel = 64;
        public const decimal OEM_ID = 88;
        public const decimal OS_VERSION = 131;
        public const decimal MODEL_NAME = 133;
        public const decimal PACKING_BATCH_VERSION = 142;

        //Product Config Type->For Bom
        public const decimal BOM_ITEM = 4;
        public const decimal BOM_PROD_TYPE = 5;
        public const decimal BOM_PART_NUMBER = 21;
        public const decimal BOM_MAC_PN = 34;
        public const decimal BOM_WWID_PN = 35;
        public const decimal BOM_CMMBD_PN = 36;
        public const decimal BOM_ITEM_DESCRIPTION = 49;
        public const decimal BOM_CPU_PART_NO = 72;
        public const decimal BOM_FJ_PN = 87;
        public const decimal BOM_PROD_TYPE1 = 89;
        public const decimal BOM_MANUFACTURE_BOARD_REVISION = 91;
        public const decimal BOM_GREEN_SYSTEM = 92;
        public const decimal BOM_OS_REV = 93;
        public const decimal BOM_OEM_ID = 94;
        public const decimal BOM_ASSET_TAG = 107;
        public const decimal BOM_ORACLE_MAC = 116;
        public const decimal BOM_ETHERNET_ADDR_CUSTOMEIZE = 120;
        public const decimal BOM_FAMILY = 126;
        public const decimal BOM_SUB_FAMILY = 127;
        public const decimal BOM_BUILD_STAGE = 130;
        public const decimal BOM_PROD_STAGE = 132;
        public const decimal BOM_CRD = 137;

        // Tyan Box Carton Label Parameter
        public const decimal BARCODE_TYAN_BOX_CARTON_MODEL = 42;
        public const decimal BARCODE_TYAN_BOX_CARTON_UPC = 43;
        public const decimal BARCODE_TYAN_BOX_CARTON_CUSTOMER = 44;
        public const decimal BARCODE_TYAN_BOX_CARTON_CUSTOMER_PN = 45;
        public const decimal BARCODE_TYAN_BOX_CARTON_DESCRIPTION1 = 46;
        public const decimal BARCODE_TYAN_BOX_CARTON_DESCRIPTION2 = 47;
        public const decimal BARCODE_TYAN_BOX_CARTON_DESCRIPTION3 = 48;

        public const decimal LNA_REMARK = 50;
        public const decimal INTEL_MM = 51;
        public const decimal INTEL_LABEL_TYPE = 52;
        public const decimal INTEL_ATTACH = 53;
        public const decimal LINK_AK_FLAG = 54;
        public const decimal BOX_SIZE = 55;
        public const decimal ID_LABEL = 56;
        public const decimal BOSE_PN = 57;
        public const decimal BOSE_PN_DESC = 58;
        public const decimal BOSE_BAIC_PN = 59;
        public const decimal BOSE_SUPPLIER_CODE = 60;
        public const decimal BOSE_PLATFORM_CODE = 61;
        public const decimal BOSE_CUSTOMER_PN = 98;

        //Intel Box/Carton Label Parameter
        public const decimal EAN = 62;
        public const decimal PRODUCT_CODE = 63;
        public const decimal RELATION_MODEL = 64;
        public const decimal INTEL_CUSTOMER_PN = 65;
        public const decimal INTEL_BOX_DESCRIPTION = 66;
        public const decimal INTEL_CARTON_KCC = 67;
        public const decimal INTEL_CARTON_PRINT_QTY = 109;

        public const decimal INTEL_MAC_OUI = 68;
        public const decimal INTEL_MAC_PART_NUMBER = 69;
        public const decimal INTEL_EUI64_OUI = 70;
        public const decimal INTEL_EUI64_PART_NUMBER = 71;

        public const decimal CARTON_UPC = 73;
        public const decimal CARTON_EAN = 74;
        public const decimal DH81PN = 75;
        public const decimal DH81REV = 76;
        public const decimal FATHER_PN = 77;
        public const decimal FATHER_REV = 78;
        public const decimal FRU_REV = 79;
        public const decimal FRU_TYPE = 81;
        public const decimal SHIP_MARK = 82;

        public const decimal PCB_Dothill_MAC_Header = 83;
        public const decimal ORACLE_MKT_PN = 84;
        public const decimal BC_PLATFORM = 85;
        public const decimal ORACLE_FRU_PN = 86;
        public const decimal CONTAINER_CAPACITY = 95;
        public const decimal COLOR = 99;

        //Mitac Box/Carton Label Parameter
        public const decimal PROJECT_CODE = 96;
        public const decimal MITAC_BOX_LABEL_DESCRIPTION = 97;

        // BBB File Product Info
        public const decimal TOP_ASSEMBLY = 100;

        // Barcode CVTE
        public const decimal BARCODE_CVTE_CARTON_EAN = 101;
        public const decimal BARCODE_CVTE_SN_EAN = 102;
        public const decimal BARCODE_CVTE_CUSTOMER_MODEL = 103;
        public const decimal BARCODE_CVTE_CUSTOMER_MODEL_PACK = 104;

        public const decimal BARCODE_INPUT_RATING1 = 105;
        public const decimal BARCODE_INPUT_RATING2 = 106;

        public const decimal REMARK_CONFIG = 108;
        //Barcode Intel 
        public const decimal QTY_CONFIG = 110;
        public const decimal BARCODE_PCBA_REVISION = 111;

        //Barcode TEC
        public const decimal PRODUCTION_NO = 112;

        //ProductFixtureConfig
        public const decimal PRODUCT_FIXTURE_FORMAT = 112;

        public const decimal GROSS_WEIGHT = 113;
        public const decimal PALLET_WEIGHT = 114;

        public const decimal BARCODE_PBA = 115;

        //For XSIGOMODULE
        public const decimal BARCODE_XSIGOMODULE_PN = 117;
        public const decimal BARCODE_XSIGOMODULE_DESCRIPTION1 = 118;
        public const decimal BARCODE_XSIGOMODULE_DESCRIPTION2 = 119;

        //For BCM

        public const decimal BARCODE_71471_IA = 121;
        public const decimal BARCODE_PWA = 122;
        public const decimal BARCODE_PWB = 123;

        //Security Operation Cage/Lock Formate

        public const decimal PRODUCT_CAGE_FORMATE = 128;
        public const decimal PRODUCT_LOCK_FORMATE = 129;

        //Logic Ultra Mac product config type
        public const decimal FCC_ID1 = 138;
        public const decimal FCC_ID2 = 139;
        public const decimal IC1 = 140;
        public const decimal ID2 = 141;

        #endregion

        //For CIS 
        public const decimal CIS_PRODUCT_NAME = 6;
        public const decimal ORACLE_PN = 7;
        public const decimal CIS_MARKING = 8;
        public const decimal REVIEION = 18;

        //For Barcode Print
        public const decimal BARCODE_TYAN_CTO_TITLE = 9;
        public const decimal BARCODE_TYAN_CTO_SUBTITLE = 10;
        public const decimal BARCODE_TYAN_CTO_MODEL = 11;
        public const decimal BARCODE_TYAN_CTO_DESCRIPTION = 12;
        public const decimal BARCODE_TYAN_CTO_REVISION = 13;
        public const decimal BARCODE_TYAN_CTO_ASSEMBLY_PN = 14;
        public const decimal BARCODE_TYAN_CTO_COMPLIANCE = 15;
        public const decimal BARCODE_UP_CODE = 17;
        public const decimal BARCODE_TYAN_CTO_CONFIG_ITEM = 41;

        public const decimal HOLD_OPERATIION_SITE = 145;

        #region Product Object Config Type
        //Product Object Config Type
        public const decimal SN0 = 1;
        public const decimal SN1 = 2;
        public const decimal CIS_NETWORK_INTERFACE_TYPE = 3;
        public const decimal CIS_COMPONENT_PRINT = 4;
        public const decimal LABEL_PRINT_QUANTITY = 5;
        public const decimal CHECK_UID_SERIALIZED = 6;
        public const decimal CHECK_FIRST_UID_ODD = 7;
        public const decimal CHECK_FIRST_UID_EVEN = 8;
        public const decimal AUTO_CREATE_UID_NUMBER = 9;
        public const decimal Check_Object_SN_Ranger = 10;
        public const decimal Check_MB_Link_Unique_UID = 11;
        public const decimal CheckComonentPartNumber = 12;
        public const decimal Get_BmcMac_By_Setup_OrderNO = 13;
        #endregion

        //Operation Config Type
        public const decimal AutoLink = 2;
        public const decimal ControlSNReplaceOperation = 4;
        public const decimal ControlComponentReplaceOperation = 7;
        public const decimal ControlCheckPicNumOperation = 8;
        public const decimal CheckAKWeight = 5;
        public const decimal FullCartonChangeComponent = 6;
        public const decimal QuicklyRepairReturnProcessSetting = 9;

        //Site Config Type 
        public const decimal Auto_Load = 1;
        public const decimal Machine_Name = 7;

        //job class
        public const decimal BoxLabelJob = 2;
        public const decimal CartonLabelJob = 3;
        public const decimal PalletLabelJob = 4;

        //ojbect config type id
        public const decimal MACTypeID = 1;


        // MONITOR_MODE
        public const decimal OnWIPQuantityMonitor = 1;
        public const decimal RepairWIPQuantityMonitor = 2;
        public const decimal WIPTimeMonitor = 3;
        public const decimal LaserPrintMonitor = 4;
        public const decimal WIPOverTimeMonitor = 5;

        private const int ReleaseProductBySerialNumber = 0;
        private const int ReleaseProductByComponentSerialNumber = 1;
        private const int ReleaseProductByCarton = 2;
        private const int ReleaseProductByPallet = 3;

        // OPERATION_CATEGORY
        public const decimal OPERATION_CATEGORY_PRINT = 10;
        public const decimal OPERATION_CATEGORY_REPAIR = 7;
        public const decimal OPERATION_CATEGORY_PACKING = 5;
        public const decimal OPERATION_CATEGORY_TURNIN = 8;
        public const decimal OPERATION_CATEGORY_TU = 3;
        public const decimal OPERATION_CATEGORY_OQA = 6;
        public const decimal OPERATION_CATEGORY_ICT_TEST = 21;
        public const decimal OPERATION_SEND_DATA = 22;
        public const decimal OPERATION_CATEGORY_WIFI_TEST = 24;
        public const decimal OPERATION_CATEGORY_TEST = 4;
        public const decimal OPERATION_CATEGORY_PALLET = 31;

        #region runcard status
        public const decimal Pass = 1;
        public const decimal Fail = 2;
        public const decimal TurnIN = 4;
        public const decimal Shipped = 5;
        public const decimal LossScan = 6;
        public const decimal Rework = 10;
        public const decimal Repaired = 8;
        public const decimal Replaced = 9;
        public const decimal CartonReplaced = 11;
        public const decimal PalletRepalced = 12;
        public const decimal WOReplaced = 13;
        public const decimal Scrapped = 7;
        public const decimal WipScrapped = 14;
        public const decimal RepairOut = 15;
        public const decimal RepairIn = 16;
        public const decimal RepairScrapped = 17;
        public const decimal QuicklyRepaired = 18;
        public const decimal WasScaned = 19;
        public const decimal NeedClear = 20;
        public const decimal WaitConfirmClear = 21;
        public const decimal RepairPICheck = 22;
        public const decimal RepairOQACheck = 23;
        #endregion

        #region Repair BGA
        public const string FAMILY_ID = "FAMILY_ID";
        public const string LOCATION = "LOCATION";
        #endregion

        public const int All = 0;
        public const int GroupByModel = 1;
        public const int GroupByPN = 2;
        public const int GroupByWo = 3;
        public const int GroupBySite = 4;
        public const int GroupByTime = 5;
        public const int GroupByWoPN = 6;
        public const int OrderByModel = 7;
        public const int OrderByPN = 8;
        public const int OrderByWo = 9;
        public const int OrderBySite = 10;
        public const int OrderByTime = 11;
        public const int GroupByCarton = 12;
        public const int GroupByPallet = 13;

        //public const string All = "All";
        //public const string GroupByModel = "Group by Model";
        //public const string GroupByPN = "Group by PN";
        //public const string GroupByWo = "Group by WO";
        //public const string GroupBySite = "Group by Site";
        //public const string GroupByTime = "Group by Time";
        //public const string GroupByWoPN = "Group by WO,PN";
        //public const string OrderByModel = "Order by Model";
        //public const string OrderByPN = "Order by PN";
        //public const string OrderByWo = "Order by WO";
        //public const string OrderBySite = "Order by Site";
        //public const string OrderByTime = "Order by Time";
        //public const string GroupByCarton = "Group by Carton";
        //public const string GroupByPallet = "Group by Pallet";

        // SFCS_PARAMETERS PRODUCT_CONFIG_TYPE
        public const string PrintHeader = "VAL";
        public const int CartonPrintData = 201;
        public const int SNPrintData = 200;

        //规则类型 0: 流水号规则 1: 箱号规则 2: 栈板号规则
        public const int RangerSN = 0;
        public const int RangerCartonNo = 1;
        public const int RangerPallectNo = 2;

        #region IS_LIKE
        public const int IsLike = 1;
        public const int NotLike = 0;
        #endregion

        #region Parameters lookup type

        public const string STOPLINE_MODE = "STOPLINE_MODE";

        // Product StopLine
        public const string STOP_OPERATION_CODE = "STOP_OPERATION_CODE";
        public const string INPUT_OPERATION_CODE = "INPUT_OPERATION_CODE";

        #endregion

        public const string PassStatus = "PASS";
        public const string FailStatus = "FAIL";
        public const string RepassStatus = "REPASS";

        // RECORD_STATUS
        public const decimal RecordProcessing = 0;
        public const decimal RecordActive = 1;
        public const decimal RecordClosed = 2;


        public const decimal DecimalDefaults = -1;

        //// StopLine Column
        //public const string DATA_VALUE = "DATA_VALUE";
        //public const string VALUE_COUNT = "VALUE_COUNT";
        //public const string HEADER_ID = "HEADER_ID";
        //public const string COLLECT_TIME = "COLLECT_TIME";
        //public const string INPUT_CONTROL = "INPUT_CONTROL";

        ////Sort
        //public const string ASC = "ASC";
        //public const string DESC = "DESC";

        // StopLine Param
        public const string NEW_STATUS = "NEW_STATUS";
        public const string NTF = "NTF";

        // StopLine Fail Statistics Type
        public const decimal NDFCount = 1;

        // StopLine Issue Type
        public const decimal StopLineAlarm = 1;
        public const decimal StopLineStop = 2;

        // Process Status
        public const int ProcessContinued = 0;
        public const int ProcessAborted = 1;

        #region laser
        public const int CODE_FAIL = 0;
        public const int CODE_PASS = 1;
        public const String FUN_TYPE_START = "START";
        public const String FUN_TYPE_GETDATA = "GETDATA";
        public const String FUN_TYPE_UPLOADEDATA = "UPLOADEDATA";
        #endregion
    }
}
