﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewCrmCore.Domain.Entitys.System;
using NewCrmCore.Domain.Services.Interface;
using NewCrmCore.Domain.ValueObject;
using NewCrmCore.Infrastructure.CommonTools;
using NewLibCore.Data.SQL.Mapper;
using NewLibCore.Validate;

namespace NewCrmCore.Domain.Services.BoundedContext
{
    public class WallpaperContext : IWallpaperContext
    {
        public async Task<(Int32 wapperId, String url)> AddWallpaperAsync(Wallpaper wallpaper)
        {
            Parameter.Validate(wallpaper);
            return await Task.Run(() =>
             {
                 using (var mapper = EntityMapper.CreateMapper())
                 {
                     #region 前置条件验证
                     {
                         var result = mapper.Select<Wallpaper>().Where(w => w.UserId == wallpaper.UserId).Count();
                         if (result > 6)
                         {
                             throw new BusinessException("最多只能上传6张图片");
                         }
                     }
                     #endregion

                     #region 插入壁纸
                     {
                         wallpaper = mapper.Add(wallpaper);
                         return (wallpaper.Id, wallpaper.Url);
                     }
                     #endregion
                 }
             });
        }

        public async Task<Wallpaper> GetUploadWallpaperAsync(String md5)
        {
            Parameter.Validate(md5);

            return await Task.Run(() =>
            {
                using (var mapper = EntityMapper.CreateMapper())
                {
                    return mapper.Select<Wallpaper>(a => new
                    {
                        a.UserId,
                        a.Height,
                        a.Id,
                        a.Md5,
                        a.ShortUrl,
                        a.Source,
                        a.Title,
                        a.Url,
                        a.Width
                    }).Where(a => a.Md5 == md5).FirstOrDefault();
                }
            });
        }

        public async Task<List<Wallpaper>> GetUploadWallpaperAsync(Int32 userId)
        {
            Parameter.Validate(userId);

            return await Task.Run(() =>
            {
                using (var mapper = EntityMapper.CreateMapper())
                {
                    return mapper.Select<Wallpaper>(a => new
                    {
                        a.UserId,
                        a.Height,
                        a.Id,
                        a.Md5,
                        a.ShortUrl,
                        a.Source,
                        a.Title,
                        a.Url,
                        a.Width
                    }).Where(a => a.UserId == userId && a.Source != WallpaperSource.System).ToList();
                }
            });
        }

        public async Task<List<Wallpaper>> GetWallpapersAsync()
        {
            return await Task.Run(() =>
            {
                using (var mapper = EntityMapper.CreateMapper())
                {
                    return mapper.Select<Wallpaper>(a => new
                    {
                        a.UserId,
                        a.Height,
                        a.Id,
                        a.Md5,
                        a.ShortUrl,
                        a.Source,
                        a.Title,
                        a.Url,
                        a.Width
                    }).Where(a => a.Source == WallpaperSource.System).ToList();
                }
            });
        }

        public async Task ModifyWallpaperModeAsync(Int32 userId, String newMode)
        {
            Parameter.Validate(userId);
            Parameter.Validate(newMode);

            await Task.Run(() =>
            {
                if (Enum.TryParse(newMode, true, out WallpaperMode wallpaperMode))
                {
                    using (var mapper = EntityMapper.CreateMapper())
                    {
                        var config = new Config();
                        config.ModeTo(wallpaperMode);
                        var result = mapper.Update(config, conf => conf.UserId == userId);
                        if (!result)
                        {
                            throw new BusinessException("修改壁纸显示失败");
                        }
                    }
                }
                else
                {
                    throw new BusinessException($"无法识别的壁纸显示模式:{newMode}");
                }
            });
        }

        public async Task ModifyWallpaperAsync(Int32 userId, Int32 newWallpaperId)
        {
            Parameter.Validate(userId);
            Parameter.Validate(newWallpaperId);

            await Task.Run(() =>
            {
                using (var mapper = EntityMapper.CreateMapper())
                {
                    var config = new Config();
                    config.NotFromBing().ModifyWallpaperId(newWallpaperId);
                    var result = mapper.Update(config, conf => conf.UserId == userId);
                    if (!result)
                    {
                        throw new BusinessException("修改壁纸失败");
                    }
                }
            });
        }

        public async Task RemoveWallpaperAsync(Int32 userId, Int32 wallpaperId)
        {
            Parameter.Validate(userId);
            Parameter.Validate(wallpaperId);

            await Task.Run(() =>
            {
                using (var mapper = EntityMapper.CreateMapper())
                {
                    #region 前置条件验证
                    {
                        var result = mapper.Select<Config>().Where(a => a.UserId == userId && a.WallpaperId == wallpaperId).Count();
                        if (result > 0)
                        {
                            throw new BusinessException("当前壁纸正在使用中，不能删除");
                        }
                    }
                    #endregion

                    #region 移除壁纸
                    {
                        var wallpaper = new Wallpaper();
                        wallpaper.Remove();
                        var result = mapper.Update(wallpaper, wa => wa.Id == wallpaperId && wa.UserId == userId);
                        if (!result)
                        {
                            throw new BusinessException("移除壁纸失败");
                        }
                    }
                    #endregion
                }
            });
        }
    }
}
