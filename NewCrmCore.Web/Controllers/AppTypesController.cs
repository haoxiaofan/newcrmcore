﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewCrmCore.Application.Services.Interface;
using NewCrmCore.Dto;
using NewLibCore.Validate;

namespace NewCrmCore.Web.Controllers
{
    public class AppTypesController : BaseController
    {
        private readonly IAppServices _appServices;

        public AppTypesController(IAppServices appServices)
        {
            _appServices = appServices;
        }

        #region 类目管理

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 创建新的类目
        /// </summary>
        /// <param name="appTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreateNewAppType(Int32 appTypeId = 0)
        {
            AppTypeDto result = null;
            if (appTypeId != 0)
            {
                result = (await _appServices.GetAppTypesAsync()).FirstOrDefault(appType => appType.Id == appTypeId);
            }
            ViewData["UniqueToken"] = CreateUniqueTokenAsync(UserInfo.Id);
            return View(result);
        }

        #endregion

        #region 移除应用类型

        /// <summary>
        /// 移除应用类型
        /// </summary>
        /// <param name="appTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Remove(Int32 appTypeId)
        {
            #region 参数验证
            Parameter.Validate(appTypeId);
            #endregion

            await _appServices.RemoveAppTypeAsync(appTypeId);
            return Json(new ResponseModel
            {
                IsSuccess = true,
                Message = "删除app类型成功"
            });
        }

        #endregion

        #region 创建应用类型

        /// <summary>
        /// 创建应用类型
        /// </summary>
        /// <param name="forms"></param>
        /// <param name="appTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(IFormCollection forms, Int32 appTypeId = 0)
        {
            #region 参数验证
            Parameter.Validate(forms);
            #endregion

            var appTypeDto = WrapperAppTypeDto(forms);
            if (appTypeId == 0)
            {
                await _appServices.CreateNewAppTypeAsync(appTypeDto);
            }
            else
            {
                await _appServices.ModifyAppTypeAsync(appTypeDto, appTypeId);
            }

            return Json(new ResponseModel
            {
                IsSuccess = true,
                Message = "app类型创建成功"
            });
        }

        #endregion

        #region 检查类型名称

        /// <summary>
        /// 检查类型名称
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CheckName(String param)
        {
            #region 参数验证
            Parameter.Validate(param);
            #endregion

            var result = await _appServices.CheckAppTypeNameAsync(param);
            return Json(!result ? new { status = "y", info = "" } : new { status = "n", info = "类型名称已存在" });
        }

        #endregion

        #region 获取所有应用类型

        /// <summary>
        /// 获取所有应用类型
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTypes(Int32 pageIndex, Int32 pageSize, String searchText)
        {
            var result = (await _appServices.GetAppTypesAsync()).Where(appType => String.IsNullOrEmpty(searchText) || appType.Name.Contains(searchText)).OrderByDescending(d => d.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return Json(new ResponseModels<IList<AppTypeDto>>
            {
                Message = "app类型获取成功",
                IsSuccess = true,
                Model = result,
                TotalCount = result.Count
            });
        }

        #endregion

        #region private method

        /// <summary>
        /// 封装从页面传入的forms表单到AppTypeDto类型
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        private static AppTypeDto WrapperAppTypeDto(IFormCollection forms)
        {
            var appTypeDto = new AppTypeDto
            {
                Name = forms["val_name"]
            };

            if ((forms["id"] + "").Length > 0)
            {
                appTypeDto.Id = Int32.Parse(forms["id"]);
            }

            appTypeDto.IsSystem = Int32.Parse(forms["val_issystem"]) == 1;
            return appTypeDto;
        }

        #endregion
    }
}