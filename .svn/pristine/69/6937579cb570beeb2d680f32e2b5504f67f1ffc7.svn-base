using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using JZ.IMS.Core.Extensions;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace JZ.IMS.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class FileController : BaseController
	{
		private readonly IHostingEnvironment _hostingEnv;
		private readonly IStringLocalizer<FileController> _localizer;

		public FileController(IHostingEnvironment hostingEnv, IStringLocalizer<FileController> localizer)
		{
			_hostingEnv = hostingEnv;
			_localizer = localizer;

		}

		/// <summary>
		/// 图片上传功能
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		public ApiBaseReturn<string> UploadImage()
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			var imgFile = Request.Form.Files[0];
			long size = 0;
			string tempname = string.Empty;
			var filename = string.Empty;
			var extname = string.Empty;

			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && (imgFile == null || imgFile.FileName.IsNullOrEmpty()))
					{
						//上传失败
						ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					if (!ErrorInfo.Status)
					{
						filename = ContentDispositionHeaderValue
											.Parse(imgFile.ContentDisposition)
											.FileName
											.Trim('"');
						extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

						#region 判断后缀
						//if (!extname.ToLower().Contains("jpg") && !extname.ToLower().Contains("png") && !extname.ToLower().Contains("gif"))
						//{
						//    return Json(new { code = 1, msg = "只允许上传jpg,png,gif格式的图片.", });
						//}
						#endregion

						#region 判断大小

						long mb = imgFile.Length / 1024 / 1024; // MB
						if (mb > 1)
						{
							//"只允许上传小于 1MB 的图片."
							ErrorInfo.Set(_localizer["upload_size_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
						}
						#endregion
					}

					#endregion

					#region 保存文件并设置返回值

					if (!ErrorInfo.Status)
					{
						var filename1 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
						tempname = filename1;
						string path = AppContext.BaseDirectory;
						string dir = DateTime.Now.ToString("yyyyMMdd");
						if (!Directory.Exists(path + $@"\upload\{dir}"))
						{
							Directory.CreateDirectory(path + $@"\upload\{dir}");
						}
						
						filename = path + $@"upload\{dir}\{filename1}";
						size += imgFile.Length;
						using (FileStream fs = System.IO.File.Create(filename))
						{
							imgFile.CopyTo(fs);
							fs.Flush();
						}
						returnVM.Result = $"/upload/{dir}/{filename1}";
					}

					#endregion
				}
				catch (Exception ex)
				{
					ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
				}
			}

			#region 如果出现错误，则写错误日志并返回错误内容

			if (ErrorInfo.Status)
			{
				returnVM.ErrorInfo.Set(ErrorInfo);
				if (ErrorInfo.ErrorType == EnumErrorType.Error)
				{
					CreateErrorLog(ErrorInfo);
				}
				ErrorInfo.Clear();
			}

			#endregion

			return returnVM;
		}
	}
}